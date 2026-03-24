using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public Vector3 openPosition;   // where the door moves to
    public float speed = 2f;

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        Debug.Log("Door is opening!");
    }
}