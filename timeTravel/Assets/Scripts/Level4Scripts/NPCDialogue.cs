using UnityEngine;
using TMPro;
using System.Collections;

public class NpcDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.04f;

    [Header("Behaviour")]
    public bool lookAtPlayerWhenTalking = true;
    public float rotationSpeed = 4f;

    private bool _isTalking;
    private bool _isTyping;
    private int _dialogueIndex;
    private Coroutine _typingCoroutine;
    private Transform _playerTransform;
    private int _dialogueStartFrame;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (_isTalking && Input.GetKeyDown(KeyCode.E))
            AdvanceDialogue();

        if (_isTalking && lookAtPlayerWhenTalking && _playerTransform != null)
            LookAtPlayer();
    }

    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) _playerTransform = player.transform;

        StartDialogue();
    }

    public bool CanInteract()
    {
        return !_isTalking;
    }

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

        ShowLine(_dialogueIndex);
    }

    private void AdvanceDialogue()
    {
        if (_isTyping)
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            if (dialogueText != null && _dialogueIndex < dialogueLines.Length)
                dialogueText.text = dialogueLines[_dialogueIndex];

            _isTyping = false;
            return;
        }

        _dialogueIndex++;

        if (_dialogueIndex < dialogueLines.Length)
            ShowLine(_dialogueIndex);
        else
            EndDialogue();
    }

    private void ShowLine(int index)
    {
        if (index >= dialogueLines.Length) return;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeLine(dialogueLines[index]));
    }

    private IEnumerator TypeLine(string line)
    {
        _isTyping = true;

        if (dialogueText != null)
            dialogueText.text = "";

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