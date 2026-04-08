using UnityEngine;

public class Stealth : MonoBehaviour
{
    public bool isCrouching;
    public bool isHidden;

    private int hidingSpotCount = 0;
    
    public GameObject hiddenIcon;


    void Update()
    {
        isHidden = hidingSpotCount > 0 && isCrouching;
        if (isHidden)
        {
            hiddenIcon.SetActive(true);
        }
        else
        {
            hiddenIcon.SetActive(false);
        }
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