using UnityEngine;
using TMPro;
using System.Collections;

public class NpcDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [TextArea(2, 5)]

    //checking how many lines the NPC will have
    public string[] dialogueLines;

    //adds the panel/canvas for the dialogue to go into, and the actual text that will be shown on screen
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    //seconds between each character being typed
    public float typingSpeed = 0.04f;

    //making sure the NPC will face the player when speaking, and turn accordingly
    [Header("Behaviour")]
    public bool lookAtPlayerWhenTalking = true;
    public float rotationSpeed = 4f;

    //checking if is talking or typing
    private bool _isTalking;
    private bool _isTyping;
    private int _dialogueIndex;
    private Coroutine _typingCoroutine;

    //used for looking at player
    private Transform _playerTransform;

    private int _dialogueStartFrame;

    private void Start()
    {
        //hide the dialogue at the start so that player only sees when clicking on the NPC
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        //While talking, pressing E will either advance to the next line or exit the dialogue entirely at the end
        if (_isTalking && Input.GetKeyDown(KeyCode.E))
            AdvanceDialogue();

        if (_isTalking && lookAtPlayerWhenTalking && _playerTransform != null)
            LookAtPlayer();
    }

    public void Interact()
    {
        //finds the player for the look at rotation
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) _playerTransform = player.transform;

        StartDialogue();
    }

    //makes sure that only one conversation can exist at a time
    public bool CanInteract()
    {
        return !_isTalking;
    }

    //shows the user they can either talk or advance through the dialogue
    public string GetInteractText()
    {
        if (_isTalking) return "Press E to continue";
        return "Press E to talk";
    }

    
    private void StartDialogue()
    {
        _isTalking = true;
        _dialogueIndex = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowLine(_dialogueIndex); //display first line
    }

    private void AdvanceDialogue()
    {
        if (_isTyping)
        {
            //pressing E can skip to full line instantly
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            //snap full line into place
            if (dialogueText != null && _dialogueIndex < dialogueLines.Length)
                dialogueText.text = dialogueLines[_dialogueIndex];

            _isTyping = false;
            return;
        }

        //move to next line
        _dialogueIndex++;

        if (_dialogueIndex < dialogueLines.Length)
            ShowLine(_dialogueIndex);
        else
            EndDialogue();
    }

    // Stops any in-progress typing and starts typing out the line at the given index
    private void ShowLine(int index)
    {
        if (index >= dialogueLines.Length) return;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeLine(dialogueLines[index]));
    }

    //types out a line character by character with the set delay
    private IEnumerator TypeLine(string line)
    {
        _isTyping = true;

        if (dialogueText != null)
            dialogueText.text = ""; //clear text before starting new line

        foreach (char c in line)
        {
            if (dialogueText != null)
                dialogueText.text += c;

            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;
    }

    
    private void EndDialogue()
    {
        _isTalking = false;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";
    }

    //rotates NPC along the y axis to look at the player
    private void LookAtPlayer()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}