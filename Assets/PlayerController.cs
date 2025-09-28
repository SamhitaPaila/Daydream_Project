using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int score = 100; // Example starting score
    public float fallThreshold = -10f; // Y position below which player is considered fallen
    public int pointsToSubtract = 10; // Points to subtract on fall
    private bool hasFallen = false;
    public Text scoreText; // Reference to UI Text for score display
    private Vector3 startPosition; // Store the starting position
    private Inventory inventory; // Reference to Inventory
    private bool awaitingSacrifice = false;
    private InventoryItemType? pendingSacrifice = null;
    public UIController uiController; // Assign in Inspector
    private bool hasAddedGrassBonus = false; // Track if grass bonus was added

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // Save the start position
        UpdateScoreUI();
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("No Inventory component found on player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasFallen && transform.position.y < fallThreshold)
        {
            score -= pointsToSubtract; // Always subtract points on fall
            UpdateScoreUI();
            CheckGameOver();
            RespawnPlayer(); // Immediately respawn the player
            hasFallen = true;
            awaitingSacrifice = true;
            if (uiController != null)
            {
                uiController.ShowButtons();
            }
        }
        if (hasFallen && transform.position.y >= fallThreshold)
        {
            hasFallen = false;
        }
        // For demonstration, use keys to choose sacrifice (replace with UI in production)
        if (awaitingSacrifice && inventory != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && inventory.HasItem(InventoryItemType.Water))
                SacrificeItem(InventoryItemType.Water);
            else if (Input.GetKeyDown(KeyCode.Alpha2) && inventory.HasItem(InventoryItemType.Food))
                SacrificeItem(InventoryItemType.Food);
            else if (Input.GetKeyDown(KeyCode.Alpha3) && inventory.HasItem(InventoryItemType.Money))
                SacrificeItem(InventoryItemType.Money);
        }
        // Add one of each item when reaching grass (x >= 15)
        if (!hasAddedGrassBonus && transform.position.x >= 15f)
        {
            hasAddedGrassBonus = true;
            if (uiController != null)
            {
                uiController.AddOneToEachItem();
            }
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void CheckGameOver()
    {
        if (score <= 0)
        {
            // End game and restart scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void RespawnPlayer()
    {
        transform.position = startPosition;
        // Optionally reset velocity if using Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void SacrificeItem(InventoryItemType type)
    {
        if (!inventory.UseItem(type)) return;
        awaitingSacrifice = false;
        ApplyItemEffect(type);
        RespawnPlayer();
        Debug.Log($"Sacrificed {type}. Remaining: Water={inventory.waterCount}, Food={inventory.foodCount}, Money={inventory.moneyCount}");
    }

    private void ApplyItemEffect(InventoryItemType type)
    {
        switch (type)
        {
            case InventoryItemType.Water:
                // Increase jump height (example: set a jumpHeight variable if you have one)
                // Example: jumpHeight += 2f;
                Debug.Log("Water used: Jump height increased!");
                break;
            case InventoryItemType.Food:
                // Increase move speed (example: set a moveSpeed variable if you have one)
                // Example: moveSpeed += 2f;
                Debug.Log("Food used: Move speed increased!");
                break;
            case InventoryItemType.Money:
                score += 10;
                UpdateScoreUI();
                Debug.Log("Money used: 10 points restored!");
                break;
        }
        CheckGameOver();
    }

    // Add this method for UI button integration
    public void ChooseSacrificeFromUI(InventoryItemType type)
    {
        if (inventory == null) return;
        if (!awaitingSacrifice) return;
        SacrificeItem(type);
    }
}
