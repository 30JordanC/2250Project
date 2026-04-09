using UnityEngine;

public class RunePuzzle : MonoBehaviour, IInteractable
{
    public static int activatedCount = 0;
    public static int totalRunes = 3;

    public int runeOrder = 1;

    private bool activated = false;
    private Renderer runeRenderer;
    private Material runeMaterial; // own copy of material

    void Start()
    {
        runeRenderer = GetComponent<Renderer>();
        runeMaterial = runeRenderer.material; // creates own copy
        
        if (runeOrder == 1) //rune order 
            runeMaterial.color = Color.cyan;
        else
            runeMaterial.color = Color.red;
    }

    void Update()
    {
        // Update locked runes to green when they become available
        if (!activated && activatedCount >= runeOrder - 1)
            runeMaterial.color = Color.cyan;
    }

    public bool CanInteract() => !activated && activatedCount >= runeOrder - 1; //checks if previous rune was activated

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
        runeMaterial.color = Color.green;

        Debug.Log("Runes: " + activatedCount + "/" + totalRunes);

        if (activatedCount >= totalRunes)
            PuzzleComplete();
    }

    void PuzzleComplete()
    {
        Debug.Log("All runes activated! Artifact unlocked!");
        GameObject artifact = GameObject.Find("Artifact");
        if (artifact != null) artifact.SetActive(true);
    }
}