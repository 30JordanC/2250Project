using UnityEngine;

public class RunePuzzle : MonoBehaviour, IInteractable
{
    public static int activatedCount = 0;
    public static int totalRunes = 3;

    [Header("Order")]
    public int runeOrder = 1; // set 1, 2, or 3 in Inspector for each rune

    private bool activated = false;
    private Renderer runeRenderer;

    void Start()
    {
        runeRenderer = GetComponent<Renderer>();
        
        // If this isn't the first rune, make it visually locked
        if (runeOrder > 1)
            runeRenderer.material.color = Color.red;
        else
            runeRenderer.material.color = Color.cyan;
    }

    public bool CanInteract() => !activated && activatedCount >= runeOrder - 1;
    
    public string GetInteractText()
    {
        if (activatedCount < runeOrder - 1)
            return "Find the previous rune first...";
        return "Press E to activate rune";
    }

    public void Interact()
    {
        if (activated) return;
        if (activatedCount < runeOrder - 1) return;
        
        activated = true;
        activatedCount++;

        runeRenderer.material.color = Color.green;
        Debug.Log("Runes: " + activatedCount + "/" + totalRunes);

        if (activatedCount >= totalRunes)
            PuzzleComplete();
        else
            Debug.Log("Find the next rune!");
    }

    void PuzzleComplete()
    {
        Debug.Log("All runes activated! Artifact unlocked!");
        GameObject artifact = GameObject.Find("Artifact");
        if (artifact != null) artifact.SetActive(true);
    }
}