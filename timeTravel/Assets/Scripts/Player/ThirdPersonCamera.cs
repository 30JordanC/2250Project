using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform orientation;

    public Transform player;

    public Transform playerObject;

    public Rigidbody rb;

    public float rotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDirection =
            player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);
            Quaternion modelOffset = Quaternion.Euler(0f, 180f, 0f);

            playerObject.rotation = Quaternion.Slerp(
                playerObject.rotation,
                targetRotation * modelOffset,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
