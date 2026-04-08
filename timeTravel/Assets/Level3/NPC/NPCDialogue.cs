using UnityEngine;
using TMPro;
//this is for allowing the npc to talk with the player
public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    public string[] dialogueLines;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    private int currentLine = 0;
    private bool isOpen = false;

    public bool CanInteract() => true;
    public string GetInteractText() => "Press E to talk"; //using the E key to progress 

    public void Interact()
    {
        if (!isOpen)
            OpenDialogue();
        else
            ShowNextLine();
    }

    void OpenDialogue()
    {
        isOpen = true;
        currentLine = 0;
        if (dialogueBox != null) dialogueBox.SetActive(true);
        ShowNextLine();
    }

    void ShowNextLine() //for multiple lines
    {
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
            currentLine++;
        }
        else
        {
            CloseDialogue();
        }
    }

    void CloseDialogue()
    {
        isOpen = false;
        if (dialogueBox != null) dialogueBox.SetActive(false);
        dialogueText.text = "";
        currentLine = 0;
    }
}