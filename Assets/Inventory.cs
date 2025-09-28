using System;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryItemType
{
    Water,
    Food,
    Money
}

public class Inventory : MonoBehaviour
{
    public int waterCount = 1;
    public int foodCount = 1;
    public int moneyCount = 1;

    public event Action<InventoryItemType> OnItemUsed;

    public bool HasItem(InventoryItemType type)
    {
        switch (type)
        {
            case InventoryItemType.Water: return waterCount > 0;
            case InventoryItemType.Food: return foodCount > 0;
            case InventoryItemType.Money: return moneyCount > 0;
            default: return false;
        }
    }

    public bool UseItem(InventoryItemType type)
    {
        if (!HasItem(type)) return false;
        switch (type)
        {
            case InventoryItemType.Water: waterCount--; break;
            case InventoryItemType.Food: foodCount--; break;
            case InventoryItemType.Money: moneyCount--; break;
        }
        OnItemUsed?.Invoke(type);
        return true;
    }

    public int GetItemCount(InventoryItemType type)
    {
        switch (type)
        {
            case InventoryItemType.Water: return waterCount;
            case InventoryItemType.Food: return foodCount;
            case InventoryItemType.Money: return moneyCount;
            default: return 0;
        }
    }
}
