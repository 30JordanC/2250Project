using UnityEngine;
using Cinemachine;

public class PipeWallInteract : MonoBehaviour, IInteractable
{
    public GameObject puzzleMenu;
    public PipePuzzle pipePuzzle;
    private bool menuIsOpen;
    private bool solved = false;
    public PlayerMovement playerMovement;
    public FirstPersonCamera firstPersonCam;
    public ThirdPersonCamera thirdPersonCam;
    public CameraModeSwitcher cameraSwitcher;
    public CinemachineInputProvider freeLookInputProvider;
    public GameObject door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzleMenu.SetActive(false);
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

        puzzleMenu.SetActive(true);
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
        return !menuIsOpen && !solved;
    }

    public string GetInteractText()
    {
        return "Press E to solve puzzle";
    }

    public void CloseMenu()
    {
        puzzleMenu.SetActive(false);
        menuIsOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement.enabled = true;
        firstPersonCam.canLook = true;
        thirdPersonCam.canLook = true;
        cameraSwitcher.canLook = true;
        freeLookInputProvider.enabled = true;
    }

    public void OnPuzzleSolved()
    {
        solved = true;
        CloseMenu();
        // Put your reward here — unlock a door, play a sound, etc.
        Debug.Log("Pipe puzzle solved!");
        Destroy(door);

    }
}
