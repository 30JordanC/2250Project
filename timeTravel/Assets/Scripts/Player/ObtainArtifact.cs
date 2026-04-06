using UnityEngine;

public class ObtainArtifact : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractText()
    {
        return "Press E to pick up T. Rex egg";
    }

    public void Interact()
    {
        Destroy(gameObject);
        SceneTransitionManager.Instance.LoadScene("IntroScene", "Spawn");
    }
}
