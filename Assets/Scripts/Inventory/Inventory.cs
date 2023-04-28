using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> crystalSlots;
    [SerializeField] List<ItemSlot> tomeSlots;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, crystalSlots, tomeSlots };
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "RECOVERY", "CRYSTALS", "TOMES"
    };

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currentSlots = GetSlotsByCategory(categoryIndex);
        return currentSlots[itemIndex].Item;
    }

    public ItemBase UseItem(int itemIndex, Character selectedCharacter, int selectedCategory)
    {
        var currentSlots = GetSlotsByCategory(selectedCategory);

        var item = currentSlots[itemIndex].Item;
        bool itemUsed = item.Use(selectedCharacter);
        if (itemUsed)
        {
            if (!item.IsReusable)
            {
                RemoveItem(item, selectedCategory);
                return item;
            }
        }

        return null;
    }

    public void RemoveItem(ItemBase item, int category)
    {
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count--;
        if(itemSlot.Count == 0)
        {
            currentSlots.Remove(itemSlot);
        }

        OnUpdated?.Invoke();
    }

    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Inventory>();
    }

}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemBase Item => item;
    public int Count
    {
        get => count;
        set => count = value;
    }
}
