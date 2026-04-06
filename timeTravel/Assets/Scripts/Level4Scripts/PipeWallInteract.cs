using UnityEngine;
using Cinemachine;

public class PipeWallInteract : MonoBehaviour, IInteractable
{
    public GameObject puzzleMenu;
    public PipePuzzle pipePuzzle;
    private bool menuIsOpen;
    private bool solved = false;
    private PlayerMovement playerMovement;
    private FirstPersonCamera firstPersonCam;
    private ThirdPersonCamera thirdPersonCam;
    private CameraModeSwitcher cameraSwitcher;
    private CinemachineInputProvider freeLookInputProvider;
    public GameObject door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzleMenu.SetActive(false);
    menuIsOpen = false;

    // Find player components at runtime
    GameObject player = GameObject.FindWithTag("Player");
    if (player != null)
    {
        playerMovement = player.GetComponentInChildren<PlayerMovement>();
        firstPersonCam = player.GetComponentInChildren<FirstPersonCamera>();
        thirdPersonCam = player.GetComponentInChildren<ThirdPersonCamera>();
        cameraSwitcher = player.GetComponentInChildren<CameraModeSwitcher>();
        freeLookInputProvider = player.GetComponentInChildren<CinemachineInputProvider>();
    }
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

    Debug.Log("Cursor unlocked: " + Cursor.lockState);
    Debug.Log("PlayerMovement null: " + (playerMovement == null));
    Debug.Log("FreeLook null: " + (freeLookInputProvider == null));

    // only disable if not null
    if (playerMovement != null) { playerMovement.StopMovement(); playerMovement.enabled = false; }
    if (firstPersonCam != null) firstPersonCam.canLook = false;
    if (thirdPersonCam != null) thirdPersonCam.canLook = false;
    if (cameraSwitcher != null) cameraSwitcher.canLook = false;
    if (freeLookInputProvider != null) freeLookInputProvider.enabled = false;
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

    if (playerMovement != null) playerMovement.enabled = true;
    if (firstPersonCam != null) firstPersonCam.canLook = true;
    if (thirdPersonCam != null) thirdPersonCam.canLook = true;
    if (cameraSwitcher != null) cameraSwitcher.canLook = true;
    if (freeLookInputProvider != null) freeLookInputProvider.enabled = true;
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
