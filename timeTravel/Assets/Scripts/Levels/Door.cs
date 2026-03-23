using UnityEngine;

public class Door : MonoBehaviour
{
    public void OpenDoor()
    {
        Debug.Log("Door opened!");
        transform.Translate(Vector3.up * 5); // simple open animation
    }
}