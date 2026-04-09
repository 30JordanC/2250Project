using UnityEngine;

public class RunePuzzle : MonoBehaviour, IInteractable //for the puzzle part
{
    public static int activatedCount = 0;
    public static int totalRunes = 3;

    private bool activated = false;
    private Renderer runeRenderer;

    void Start()
    {
        runeRenderer = GetComponent<Renderer>();
    }

    public bool CanInteract() => !activated;
    public string GetInteractText() => "Press E to activate rune"; //player interaction

    public void Interact()
    {
        if (activated) return;
        activated = true;
        activatedCount++;

        if (runeRenderer != null)
            runeRenderer.material.color = Color.green;

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