using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform orientation;

    public Transform player;

    public Transform playerObject1;
    public Transform playerObject2;
    public Transform playerObject3;

    public Rigidbody rb;

    public float rotationSpeed;

    public bool canLook = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canLook) return;
        
        Vector3 viewDirection =
            player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);
            Quaternion modelOffset = Quaternion.Euler(0f, 0f, 0f);

            playerObject1.rotation = Quaternion.Slerp(
                playerObject1.rotation,
                targetRotation * modelOffset,
                rotationSpeed * Time.deltaTime
            );
            
            playerObject2.rotation = Quaternion.Slerp(
                playerObject2.rotation,
                targetRotation * modelOffset,
                rotationSpeed * Time.deltaTime
            );
            
            playerObject3.rotation = Quaternion.Slerp(
                playerObject3.rotation,
                targetRotation * modelOffset,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
