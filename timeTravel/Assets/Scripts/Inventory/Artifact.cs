using UnityEngine;

public class Artifact : Item
{
    [SerializeField] private bool isCollected = false;

    public bool IsCollected => isCollected;

    public override void Pickup(Inventory inventory)
    {
        if (inventory == null) return;

        bool added = inventory.AddItem(this);

        if (added)
        {
            isCollected = true;
            Debug.Log(ItemName + " artifact collected.");
            gameObject.SetActive(false);
        }
    }
}