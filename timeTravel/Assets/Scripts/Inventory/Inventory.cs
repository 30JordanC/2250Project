using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity = 8;
    [SerializeField] private List<Item> items = new List<Item>();

    public List<Item> Items => items;
    public int Capacity => capacity;

    private void Awake()
    {
        while (items.Count < capacity)
        {
            items.Add(null);
        }
    }

    public bool AddItem(Item item)
    {
        if (item == null || IsFull()) return false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                Debug.Log(item.ItemName + " added to inventory.");
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                Debug.Log(item.ItemName + " removed from inventory.");
                return true;
            }
        }

        return false;
    }

    public Item GetItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item != null && item.ItemName == itemName)
            {
                return item;
            }
        }

        return null;
    }

    public bool MoveToSlot(Item item, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= capacity || item == null)
            return false;

        int currentIndex = items.IndexOf(item);

        if (currentIndex == -1)
            return false;

        Item temp = items[slotIndex];
        items[slotIndex] = item;
        items[currentIndex] = temp;

        return true;
    }

    public bool IsFull()
    {
        foreach (Item item in items)
        {
            if (item == null)
            {
                return false;
            }
        }

        return true;
    }
}