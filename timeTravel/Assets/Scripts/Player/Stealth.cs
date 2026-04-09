using UnityEngine;

public class Stealth : MonoBehaviour
{
    public bool isCrouching;
    public bool isHidden;
    public bool stealthAbilityUnlocked = false;
    private int hidingSpotCount = 0;
    
    public GameObject hiddenIcon;
    
    public float invisibilityDuration = 5f;
    public float cooldownDuration = 20f;

    private bool invisibilityActive = false;
    private bool onCooldown = false;
    private float invisibilityTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        hiddenIcon.SetActive(false);
    }

    void Update()
    {
        if (stealthAbilityUnlocked && Input.GetKeyDown(KeyCode.F) && !invisibilityActive && !onCooldown)
        {
            invisibilityActive = true;
            invisibilityTimer = invisibilityDuration;
            onCooldown = true;
            cooldownTimer = cooldownDuration;
        }
        
        
        if (invisibilityActive)
        {
            invisibilityTimer -= Time.deltaTime;

            if (invisibilityTimer <= 0f)
            {
                invisibilityActive = false;
                invisibilityTimer = 0f;
            }
        }
        
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                onCooldown = false;
                cooldownTimer = 0f;
            }
        }

        isHidden = (hidingSpotCount > 0 && isCrouching) || invisibilityActive;

        hiddenIcon.SetActive(isHidden);
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