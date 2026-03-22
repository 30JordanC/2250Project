using Cinemachine;
using UnityEngine;

public class CameraModeSwitcher : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    public Camera firstPersonCamera;
    
    private bool isFirstPerson;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetThirdPerson();
    }

    // Update is called once per frame
    void Update()
    {
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
        freeLookCamera.gameObject.SetActive(false);
        firstPersonCamera.gameObject.SetActive(true);

    }

    void SetThirdPerson()
    {
        firstPersonCamera.gameObject.SetActive(false);
        freeLookCamera.gameObject.SetActive(true);
    }
}
