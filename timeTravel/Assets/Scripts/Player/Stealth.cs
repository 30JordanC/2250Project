using UnityEngine;

public class Stealth : MonoBehaviour
{
    public bool isCrouching;
    public bool isHidden;

    private int hidingSpotCount = 0;

    void Update()
    {
        isHidden = hidingSpotCount > 0 && isCrouching;
        Debug.Log("Hidden: " + isHidden);
    }

    public void SetCrouching(bool crouching)
    {
        isCrouching = crouching;
    }

    public void EnterHidingSpot()
    {
        hidingSpotCount++;
    }

    public void ExitHidingSpot()
    {
        hidingSpotCount = Mathf.Max(0, hidingSpotCount - 1);
    }
}