using UnityEngine;
using UnityEngine.UI;

public class SacrificePanelController : MonoBehaviour
{
    public GameObject panel;
    public Button waterButton;
    public Button foodButton;
    public Button moneyButton;
    private PlayerController playerController;

    void Start()
    {
        panel.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();

        waterButton.onClick.AddListener(() => Choose(InventoryItemType.Water));
        foodButton.onClick.AddListener(() => Choose(InventoryItemType.Food));
        moneyButton.onClick.AddListener(() => Choose(InventoryItemType.Money));
    }

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Choose(InventoryItemType type)
    {
        playerController.ChooseSacrificeFromUI(type);
        Hide();
    }
}