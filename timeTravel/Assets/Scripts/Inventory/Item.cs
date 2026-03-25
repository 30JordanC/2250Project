using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;

    public virtual void Pickup(Inventory inventory)
    {
        AddToInventory(inventory);
    }

    public virtual bool AddToInventory(Inventory inventory)
    {
        if (inventory == null) return false;

        bool added = inventory.AddItem(this);

        if (added)
        {
            gameObject.SetActive(false);
        }

        return added;
    }
}