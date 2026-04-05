using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity = 10; 
    private List<Item> items = new List<Item>();

    public int Capacity => capacity;
    public List<Item> Items => items;

    public bool AddItem(Item item)
    {
        if (item == null) return false;
        if (items.Count >= capacity) return false;
        items.Add(item);
        Debug.Log(item.ItemName + " added to inventory.");
        return true;
    }

    public bool RemoveItem(Item item)
    {
        if (item == null) return false;
        bool removed = items.Remove(item);
        if (removed)
            Debug.Log(item.ItemName + " removed from inventory.");
        return removed;
    }

    public Item GetItem(string searchName)
    {
        foreach (Item item in items)
            if (item != null && item.ItemName == searchName)
                return item;
        return null;
    }

    public bool MoveToSlot(Item item, int slotIndex)
    {
        if (item == null) return false;
        if (slotIndex < 0 || slotIndex >= items.Count) return false;
        int currentIndex = items.IndexOf(item);
        if (currentIndex == -1) return false;
        Item temp = items[slotIndex];
        items[slotIndex] = items[currentIndex];
        items[currentIndex] = temp;
        return true;
    }

    public bool IsFull() => items.Count >= capacity;
}