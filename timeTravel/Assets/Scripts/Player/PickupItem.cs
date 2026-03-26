using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Destroy(gameObject);
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractText()
    {
        return "Press E to pick up";
    }
}
