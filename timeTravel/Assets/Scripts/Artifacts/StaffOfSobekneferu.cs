using UnityEngine;

public class StaffOfSobekneferu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Staff collected!");

            GameControl.Instance.currentLevel.EndLevel();

            Destroy(gameObject);
        }
    }
}