using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject buttonPanel; // Parent GameObject containing the buttons
    public Button waterButton;
    public Button foodButton;
    public Button moneyButton;
    private Inventory playerInventory;

    void Start()
    {
        buttonPanel.SetActive(false);
        // Find the Inventory component on the player
        playerInventory = FindObjectOfType<Inventory>();

        waterButton.onClick.AddListener(() => SacrificeItem(InventoryItemType.Water));
        foodButton.onClick.AddListener(() => SacrificeItem(InventoryItemType.Food));
        moneyButton.onClick.AddListener(() => SacrificeItem(InventoryItemType.Money));
    }

    public void ShowButtons()
    {
        buttonPanel.SetActive(true);
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
                playerInventory.waterCount = 0;
                break;
            case InventoryItemType.Food:
                playerInventory.foodCount = 0;
                break;
            case InventoryItemType.Money:
                playerInventory.moneyCount = 0;
                break;
        }
        HideButtons();
    }
}