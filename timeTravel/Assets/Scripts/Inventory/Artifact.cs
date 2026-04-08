using UnityEngine;

public class Artifact : Item
{
    public override void Pickup(Inventory inventory)
    {
        if (inventory == null) return;
        inventory.AddItem(this);
        Debug.Log(ItemName + " artifact collected.");
        
        if (HotbarUI.Instance != null)
            HotbarUI.Instance.ForceRefresh();
    }
}