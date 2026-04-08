using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
        if (item == null)
            Debug.LogWarning(gameObject.name + " is missing an Item component.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Inventory inventory = other.GetComponent<Inventory>() 
                           ?? other.GetComponentInParent<Inventory>()
                           ?? GameObject.FindGameObjectWithTag("Player")
                                        .GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("Could not find Inventory on player!");
            return;
        }

        if (item != null)
        {
            item.Pickup(inventory);

            // Tell the hotbar to refresh so the item appears in the UI right away
            if (HotbarUI.Instance != null)
                HotbarUI.Instance.ForceRefresh();
        }
    }
}