using Cinemachine;
using UnityEngine;

public class CameraModeSwitcher : MonoBehaviour
{
    public Camera thirdPersonCamera;

    public Camera firstPersonCamera;
    
    private bool isFirstPerson;

    public bool canLook = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetThirdPerson();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canLook) return;
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            isFirstPerson = !isFirstPerson;
            
            if (isFirstPerson) 
            {
                SetFirstPerson();
            } else {
                SetThirdPerson();
            }
        }
    }

    void SetFirstPerson()
    {
        thirdPersonCamera.gameObject.SetActive(false);
        firstPersonCamera.gameObject.SetActive(true);

    }

    void SetThirdPerson()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
    }
}
