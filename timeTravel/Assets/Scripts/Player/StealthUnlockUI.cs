using UnityEngine;

public class StealthUnlockUI : MonoBehaviour
{
    
    [SerializeField] private GameObject stealthUnlockPanel;
    private PlayerReferences playerRefs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRefs = FindFirstObjectByType<PlayerReferences>();
        if (stealthUnlockPanel != null)
        {
            stealthUnlockPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stealthUnlockPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HideStealthUnlockPanel();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    public void ShowStealthUnlockPanel()
    {
        if (playerRefs == null)
            playerRefs = FindFirstObjectByType<PlayerReferences>();

        stealthUnlockPanel.SetActive(true);

        if (playerRefs != null)
        {
            if (playerRefs.playerMovement != null)
            {
                playerRefs.playerMovement.StopMovement();
                playerRefs.playerMovement.enabled = false;
            }

            if (playerRefs.firstPersonCam != null)
                playerRefs.firstPersonCam.canLook = false;

            if (playerRefs.thirdPersonCam != null)
                playerRefs.thirdPersonCam.canLook = false;

            if (playerRefs.cameraSwitcher != null)
                playerRefs.cameraSwitcher.canLook = false;

            if (playerRefs.freeLookInputProvider != null)
                playerRefs.freeLookInputProvider.enabled = false;
        }
    }
    
    public void HideStealthUnlockPanel()
    {
        if (playerRefs == null)
            playerRefs = FindFirstObjectByType<PlayerReferences>();

        stealthUnlockPanel.SetActive(false);

        if (playerRefs != null)
        {
            if (playerRefs.playerMovement != null)
                playerRefs.playerMovement.enabled = true;

            if (playerRefs.firstPersonCam != null)
                playerRefs.firstPersonCam.canLook = true;

            if (playerRefs.thirdPersonCam != null)
                playerRefs.thirdPersonCam.canLook = true;

            if (playerRefs.cameraSwitcher != null)
                playerRefs.cameraSwitcher.canLook = true;

            if (playerRefs.freeLookInputProvider != null)
                playerRefs.freeLookInputProvider.enabled = true;
        }
    }
}
