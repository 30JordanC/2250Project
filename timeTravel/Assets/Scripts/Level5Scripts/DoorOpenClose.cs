using UnityEngine;
using System.Collections;

public class DoorOpenClose : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public Vector3 leftOpenOffset = new Vector3(-2f,0,0);
    public Vector3 rightOpenOffset = new Vector3(2f,0,0);
    public float moveSpeed = 2f;        // units per second
    public float stayOpenTime = 3f;

    public AudioSource leftDoorSound;   // attach AudioSource from LeftDoor
    public AudioSource rightDoorSound;  // attach AudioSource from RightDoor

    private Vector3 leftStartPos;
    private Vector3 rightStartPos;
    private Vector3 leftTargetPos;
    private Vector3 rightTargetPos;

    private bool isMoving = false;
    private bool isOpening = false;

    void Start()
    {
        leftStartPos = leftDoor.position;
        rightStartPos = rightDoor.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // move doors toward target at constant speed
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTargetPos, moveSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTargetPos, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            StartCoroutine(OpenThenClose());
        }
    }

    IEnumerator OpenThenClose()
    {
        isMoving = true;

        // --- OPEN ---
        isOpening = true;
        leftTargetPos = leftStartPos + leftOpenOffset;
        rightTargetPos = rightStartPos + rightOpenOffset;
        PlayDoorSound();

        // wait until doors reach fully open positions
        while (Vector3.Distance(leftDoor.position, leftTargetPos) > 0.01f ||
               Vector3.Distance(rightDoor.position, rightTargetPos) > 0.01f)
        {
            yield return null; // wait until next frame
        }

        // stay open for a bit
        yield return new WaitForSeconds(stayOpenTime);

        // --- CLOSE ---
        isOpening = false;
        leftTargetPos = leftStartPos;
        rightTargetPos = rightStartPos;
        PlayDoorSound();

        // wait until doors fully close
        while (Vector3.Distance(leftDoor.position, leftTargetPos) > 0.01f ||
               Vector3.Distance(rightDoor.position, rightTargetPos) > 0.01f)
        {
            yield return null;
        }

        isMoving = false;
    }

    void PlayDoorSound()
    {
        if (leftDoorSound != null) leftDoorSound.Play();
        if (rightDoorSound != null) rightDoorSound.Play();
    }
}