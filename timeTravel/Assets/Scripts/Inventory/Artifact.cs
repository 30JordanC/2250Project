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

            // Tell the hotbar UI to refresh so icon appears immediately this is good
            if (HotbarUI.Instance != null)
                HotbarUI.Instance.ForceRefresh();

            gameObject.SetActive(false);
        }
    }
}