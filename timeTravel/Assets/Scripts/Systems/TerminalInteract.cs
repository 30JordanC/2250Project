using Cinemachine;
using UnityEngine;

public class TerminalInteract : MonoBehaviour, IInteractable
{
    public GameObject terminalMenu;
    public bool menuIsOpen;

    public PlayerMovement playerMovement;
    public FirstPersonCamera firstPersonCam;
    public ThirdPersonCamera thirdPersonCam;
    public CameraModeSwitcher cameraSwitcher;

    public CinemachineInputProvider freeLookInputProvider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terminalMenu.SetActive(false);
        menuIsOpen = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuIsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void Interact()
    {
        if (menuIsOpen) return;
        
        terminalMenu.SetActive(true);
        menuIsOpen = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerMovement.StopMovement();
        playerMovement.enabled = false;
        firstPersonCam.canLook = false;
        thirdPersonCam.canLook = false;
        cameraSwitcher.canLook = false;
   
        freeLookInputProvider.enabled = false;
    }

    public bool CanInteract()
    {
        return !menuIsOpen;
    }

    public string GetInteractText()
    {
        return "Press E to time travel";
    }

    public void CloseMenu() {
        terminalMenu.SetActive(false);
        menuIsOpen = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        playerMovement.enabled = true;
        firstPersonCam.canLook = true;
        thirdPersonCam.canLook = true;
        cameraSwitcher.canLook = true;

        freeLookInputProvider.enabled = true;
    }
    
    public void GoToLevel1()
    {
        CloseMenu();
        SceneTransitionManager.Instance.LoadScene("Level 1", "Level1Spawn");
    }


}
