using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private int groundContacts = 0;
    private bool isGrounded => groundContacts > 0;

    // Game state variables
    public float progress = 0f;
    public int money = 5, food = 3, water = 3, special = 1;
    public bool gameOver = false, victory = false;
    public GameObject enemyPrefab;
    public List<GameObject> enemies = new List<GameObject>();
    public float enemySpawnInterval = 10f;
    private float nextEnemySpawn = 10f;
    public Canvas sacrificeCanvas; // Assign a UI Canvas for sacrifice
    private bool showSacrifice = false;
    private int sacrificeLevel = 0;
    private float lastSacrificeProgress = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpawnEnemies(0);
        if (sacrificeCanvas != null)
            sacrificeCanvas.enabled = false;
    }

    void Update()
    {
        if (gameOver || victory || showSacrifice) return;

        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) moveInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) moveInput = 1f;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Progress increases when moving right
        if (moveInput > 0)
        {
            progress += Time.deltaTime * 10f;
            if (progress >= 100f)
            {
                victory = true;
                Debug.Log("Victory! You survived the 100 meters!");
            }
        }

        // Sacrifice check
        int currentMilestone = Mathf.FloorToInt(progress / 20f) * 20;
        int lastMilestone = Mathf.FloorToInt(lastSacrificeProgress / 20f) * 20;
        if (currentMilestone > lastMilestone && currentMilestone > 0 && !showSacrifice)
        {
            showSacrifice = true;
            sacrificeLevel = Mathf.FloorToInt(progress / 20f);
            if (sacrificeCanvas != null)
                sacrificeCanvas.enabled = true;
            Time.timeScale = 0f; // Pause game
        }

        // Enemy spawn logic
        if (progress >= nextEnemySpawn)
        {
            SpawnEnemies(progress);
            nextEnemySpawn += enemySpawnInterval;
        }

        // Move enemies toward player
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            Vector2 dir = (transform.position - enemy.transform.position).normalized;
            enemy.transform.position += (Vector3)(dir * Time.deltaTime * 2f);
            // Check collision
            if (Vector2.Distance(transform.position, enemy.transform.position) < 1f)
            {
                gameOver = true;
                Debug.Log("Game Over! An enemy caught you!");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                groundContacts++;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        groundContacts = Mathf.Max(groundContacts - 1, 0);
    }

    void SpawnEnemies(float progress)
    {
        int enemyCount = Mathf.FloorToInt(progress / 10f) + 1;
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + 10f + i * 3f, transform.position.y, 0);
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemies.Add(enemy);
        }
    }

    // Call these from UI buttons for sacrifice
    public void Sacrifice(string type)
    {
        if (type == "money" && money > 0) money--;
        else if (type == "food" && food > 0) food--;
        else if (type == "water" && water > 0) water--;
        else if (type == "special" && special > 0) special--;
        else return;

        showSacrifice = false;
        lastSacrificeProgress = progress;
        if (sacrificeCanvas != null)
            sacrificeCanvas.enabled = false;
        Time.timeScale = 1f; // Resume game
        Debug.Log($"Sacrificed {type}! Path cleared!");
    }
}