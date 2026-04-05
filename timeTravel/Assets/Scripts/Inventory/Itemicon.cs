using UnityEngine;

/// <summary>
/// Add this to any GameObject that has Item/Artifact/Weapon on it.
/// Drag a PNG sprite into the Icon field and it will show in the hotbar.
/// </summary>
public class ItemIcon : MonoBehaviour
{
    [Header("Drag your icon PNG sprite here")]
    public Sprite icon;
}