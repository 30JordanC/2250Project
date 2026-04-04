using System;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float sensitivyX;
    public float sensitivityY;
    public Transform orientation;
    public Transform playerObject1;
    public Transform playerObject2;
    public Transform playerObject3;
    
    float xRotation;
    float yRotation;

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
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivyX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        playerObject1.rotation = orientation.rotation;
        playerObject2.rotation = orientation.rotation;
        playerObject3.rotation = orientation.rotation;
    }

    private void OnEnable()
    {
        yRotation = playerObject1.rotation.eulerAngles.y;
        yRotation = playerObject2.rotation.eulerAngles.y;
        yRotation = playerObject3.rotation.eulerAngles.y;
    }
}
