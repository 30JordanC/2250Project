using UnityEngine;

public class TerminalMenuController : MonoBehaviour
{
    private PlayerReferences playerRefs;

    void Start()
    {
        playerRefs = FindFirstObjectByType<PlayerReferences>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        if (playerRefs == null)
            playerRefs = FindFirstObjectByType<PlayerReferences>();

        gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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

    public void CloseMenu()
    {
        if (playerRefs == null)
            playerRefs = FindFirstObjectByType<PlayerReferences>();

        gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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

    public void GoToLevel1()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 1", "Level1Spawn");
    }

    public void GoToLevel2()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 2_inside", "Level2Spawn");
    }

    public void GoToLevel3()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 3", "Level3Spawn");
    }

    public void GoToLevel4()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 4", "Level4Spawn");
    }

    public void GoToLevel5()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 5", "Level5Spawn");
    }

    public void GoToLevel6()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 6", "Level6Spawn");
    }
}