using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float sensitivyX;
    public float sensitivityY;
    public Transform orientation;
    public Transform playerObject;
    
    float xRotation;
    float yRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivyX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;
        
        yRotation += mouseX;
        xRotation -= mouseY;
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        playerObject.rotation = Quaternion.Euler(0, yRotation + 180f, 0);

    }

    void OnEnable()
    {
        SyncRotation();
    }

    void SyncRotation()
    {
        yRotation = orientation.eulerAngles.y;
    }
}
