using UnityEngine;

public class NodeInteract : MonoBehaviour
{
    public GameObject promptUI;
    public Light nodeLight;
    public AudioSource audioSource;

    public Renderer[] nodeRenderers; 
    
    private bool playerNearby = false;
    private bool isDisabled = false;

    [Header("Door (Manual Control)")]
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftOpenOffset = new Vector3(-2f, 0, 0);
    public Vector3 rightOpenOffset = new Vector3(2f, 0, 0);

    public float doorSpeed = 2f;

    private bool openDoor = false;
    private Vector3 leftTarget;
    private Vector3 rightTarget;

    [Header("Post-Node Lights")]
    public GameObject[] lightsToActivate;
    
    void Update()
    {
        if (playerNearby && !isDisabled && Input.GetKeyDown(KeyCode.X))
        {
            DisableNode();
        }
        
        // Move doors toward open position
        if (openDoor)
        {
            if (leftDoor != null)
            {
                leftDoor.position = Vector3.MoveTowards(
                    leftDoor.position,
                    leftTarget,
                    doorSpeed * Time.deltaTime
                );
            }

            if (rightDoor != null)
            {
                rightDoor.position = Vector3.MoveTowards(
                    rightDoor.position,
                    rightTarget,
                    doorSpeed * Time.deltaTime
                );
            }
        }
    }

    void DisableNode()
    {
        isDisabled = true;

        // Hide prompt
        if (promptUI != null)
            promptUI.SetActive(false);

        // Change light to red
        if (nodeLight != null)
            nodeLight.color = Color.red;

        // Change node color
        if (nodeRenderers != null)
        {
            foreach (Renderer r in nodeRenderers)
            {
                r.material.color = Color.red;
            }
        }

        // Play sound
        if (audioSource != null)
            audioSource.Play();

        Debug.Log("Node disabled!");
        
        // Set door targets
        if (leftDoor != null)
            leftTarget = leftDoor.position + leftOpenOffset;

        if (rightDoor != null)
            rightTarget = rightDoor.position + rightOpenOffset;

        openDoor = true;

// Turn ON new lights
        if (lightsToActivate != null)
        {
            foreach (GameObject light in lightsToActivate)
            {
                light.SetActive(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (!isDisabled && promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}