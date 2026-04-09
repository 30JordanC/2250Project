using UnityEngine;

public class ObtainArtifact : MonoBehaviour, IInteractable
{
    public string interactText = "Press E to pick up <artifactName>";
    public Transform player;

    public bool isEgg = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
        }
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
        Stealth stealth = player.GetComponent<Stealth>();
        stealth.stealthAbilityUnlocked = true;
        SceneTransitionManager.Instance.LoadScene("IntroScene", "IntroSpawn");

        if (isEgg)
        {
            StealthUnlockUI stealthUnlockUI = player.GetComponent<StealthUnlockUI>();
            if (stealthUnlockUI != null)
            {
                stealthUnlockUI.ShowStealthUnlockPanel();
            }
        }
        
    }
}
