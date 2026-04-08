using UnityEngine;

public class ArtifactAutoPickup : MonoBehaviour
{
    private bool pickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;
        pickedUp = true;

        Inventory inventory = null;
        foreach (var inv in Resources.FindObjectsOfTypeAll<Inventory>())
        { inventory = inv; break; }

        Item item = GetComponent<Item>();

        if (inventory != null && item != null)
        {
            inventory.AddItem(item);
            
            HotbarUI hotbar = null;
            foreach (var h in Resources.FindObjectsOfTypeAll<HotbarUI>())
            { hotbar = h; break; }
            if (hotbar != null) hotbar.ForceRefresh();
        }

        // Always transition regardless
        SceneTransitionManager.Instance.LoadScene("IntroScene", "IntroSpawn");
    }
}