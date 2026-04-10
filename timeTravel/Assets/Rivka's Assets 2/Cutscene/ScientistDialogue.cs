using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScientistDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
   
    public GameObject dialoguePanel; // your panel
    public GameObject promptText;    // "Press I"

    private bool playerInRange = false;
    private bool dialogueStarted = false;

    
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        dialoguePanel.SetActive(false);
        if (promptText != null)
            promptText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Show/hide prompt
        if (playerInRange && !dialogueStarted)
        {
            promptText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.I))
            {
                dialogueStarted = true;
                dialoguePanel.SetActive(true);
                promptText.SetActive(false);

                StartDialogue();
            }
        }
        else
        {
            if (promptText != null)
                promptText.SetActive(false);
        }

        // Dialogue progression (ONLY after started)
        if (dialogueStarted && Input.GetKeyDown(KeyCode.I))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            dialogueStarted = false;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptText != null)
                promptText.SetActive(false);
        }
    }
}