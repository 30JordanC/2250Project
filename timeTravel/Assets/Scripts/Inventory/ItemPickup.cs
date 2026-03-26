using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null && item != null)
        {
            item.Pickup(inventory);
        }
    }
}