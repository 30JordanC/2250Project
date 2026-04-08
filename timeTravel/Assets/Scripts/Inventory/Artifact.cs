using UnityEngine;

public class Artifact : Item
{
    public override void Pickup(Inventory inventory)
    {
        if (inventory == null) return;
<<<<<<< Updated upstream

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
=======
        inventory.AddItem(this);
>>>>>>> Stashed changes
    }
}