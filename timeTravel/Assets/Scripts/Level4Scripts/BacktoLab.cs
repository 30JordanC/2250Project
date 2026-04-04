using UnityEngine;

public class ScenePortal : MonoBehaviour, IInteractable
{
    public string sceneName;
    public string spawnID;

    public void Interact()
    {
        SceneTransitionManager.Instance.LoadScene(sceneName, spawnID);
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractText()
    {
        return "Press E to return to the lab";
    }
}