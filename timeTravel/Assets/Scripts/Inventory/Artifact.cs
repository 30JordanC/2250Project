using UnityEngine;

public class Artifact : Item
{
    [SerializeField] private bool isCollected = false;

    public bool IsCollected => isCollected;

    public override void Pickup(Inventory inventory)
    {
        bool added = AddToInventory(inventory);

        if (added)
        {
            isCollected = true;
            Debug.Log(itemName + " artifact collected.");
        }
    }
}