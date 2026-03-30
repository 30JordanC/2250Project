using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;

    public virtual void Pickup(Inventory inventory)
    {
        if (inventory == null) return;

        bool added = inventory.AddItem(this);

        if (added)
        {
            gameObject.SetActive(false);
        }
    }
}