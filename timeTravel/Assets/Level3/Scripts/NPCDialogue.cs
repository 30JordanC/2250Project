using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string[] dialogueLines;
    public TMP_Text dialogueText;
    public GameObject dialogueBox; 

    private int currentLine = 0;
    private bool playerNearby = false;

    void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false); // hidden at start
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (dialogueText == null) return;

        if (currentLine < dialogueLines.Length)
        {
            if (dialogueBox != null) dialogueBox.SetActive(true); // show box
            dialogueText.text = dialogueLines[currentLine];
            currentLine++;
        }
        else
        {
            // End of dialogue — hide the box and reset
            if (dialogueBox != null) dialogueBox.SetActive(false);
            dialogueText.text = "";
            currentLine = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player entered NPC trigger"); // test
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (dialogueBox != null) dialogueBox.SetActive(false);
            if (dialogueText != null) dialogueText.text = "";
            currentLine = 0;
        }
    }
}