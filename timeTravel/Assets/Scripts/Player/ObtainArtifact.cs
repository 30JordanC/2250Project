using UnityEngine;

public class ObtainArtifact : MonoBehaviour, IInteractable
{
    public string interactText = "Press E to pick up <artifactName>";

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
        return interactText;
    }

    public void Interact()
    {
        SceneTransitionManager.Instance.LoadScene("IntroScene", "IntroSpawn");
    }
}
