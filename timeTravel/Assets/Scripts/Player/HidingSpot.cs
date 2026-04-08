using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Stealth stealth = other.GetComponent<Stealth>();
            if (stealth != null)
            {
                stealth.EnterHidingSpot();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Stealth stealth = other.GetComponent<Stealth>();
            if (stealth != null)
            {
                stealth.ExitHidingSpot();
            }
        }
    }
}