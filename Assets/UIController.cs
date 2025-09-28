using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject buttonPanel; // Parent GameObject containing the buttons
    public Button waterButton;
    public Button foodButton;
    public Button moneyButton;
    public Text itemCountsText; // Text object to display item counts
    private Inventory playerInventory;

    void Start()
    {
        buttonPanel.SetActive(false);
        // Find the Inventory component on the player
        playerInventory = FindObjectOfType<Inventory>();
        // Ensure starting with 1 of each item
        if (playerInventory != null)
        {
            playerInventory.waterCount = 1;
            playerInventory.foodCount = 1;
            playerInventory.moneyCount = 1;
        }
        UpdateItemCountsText();
        waterButton.onClick.AddListener(() => SacrificeItem(InventoryItemType.Water));
        foodButton.onClick.AddListener(() => SacrificeItem(InventoryItemType.Food));
        moneyButton.onClick.AddListener(() => SacrificeItem(InventoryItemType.Money));
    }

    public void ShowButtons()
    {
        buttonPanel.SetActive(true);
        UpdateItemCountsText();
    }

    private void HideButtons()
    {
        buttonPanel.SetActive(false);
    }

    private void SacrificeItem(InventoryItemType type)
    {
        if (playerInventory == null) return;
        switch (type)
        {
            case InventoryItemType.Water:
                playerInventory.waterCount = Mathf.Max(0, playerInventory.waterCount - 1);
                break;
            case InventoryItemType.Food:
                playerInventory.foodCount = Mathf.Max(0, playerInventory.foodCount - 1);
                break;
            case InventoryItemType.Money:
                playerInventory.moneyCount = Mathf.Max(0, playerInventory.moneyCount - 1);
                break;
        }
        UpdateItemCountsText();
        HideButtons();
    }

    public void AddOneToEachItem()
    {
        if (playerInventory == null) return;
        playerInventory.waterCount += 1;
        playerInventory.foodCount += 1;
        playerInventory.moneyCount += 1;
        UpdateItemCountsText();
    }

    private void UpdateItemCountsText()
    {
        if (playerInventory == null || itemCountsText == null) return;
        itemCountsText.text = $"Water: {playerInventory.waterCount}\nFood: {playerInventory.foodCount}\nMoney: {playerInventory.moneyCount}";
    }
}