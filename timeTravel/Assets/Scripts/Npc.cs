using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Npc : MonoBehaviour
{
    // ── Inspector ────────────────────────────────────────────────
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines = new string[]
    {
        "HALT! I am the Guardian Golem of this realm.",
        "A dark ghost has corrupted this land with evil energy.",
        "It cannot be destroyed by ordinary means...",
        "You must find the Legendary Axe hidden on these bridges.",
        "Only its ancient power can banish the ghost forever.",
        "Find the axe. Defeat the ghost. Restore this world.",
        "Go now, traveller. Time is running out!"
    };

    [Header("Interaction")]
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI - Assign in Inspector")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text interactPromptText;
    public float typingSpeed = 0.04f;

    [Header("Npc Behaviour")]
    public bool lookAtPlayerWhenTalking = true;
    public float rotationSpeed = 4f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip talkSound;
    public AudioClip greetSound;

    [Header("Optional Animator")]
    public Animator animator;
    public string talkingBoolName = "isTalking";

    // ── Private ──────────────────────────────────────────────────
    private Transform _player;
    private int _dialogueIndex;
    private bool _isTalking;
    private bool _playerInRange;
    private bool _isTyping;
    private Coroutine _typingCoroutine;

    private static readonly int IsTalkingHash = Animator.StringToHash("isTalking");
    private static readonly int IdleActionHash = Animator.StringToHash("IdleAction");

    // ─────────────────────────────────────────────────────────────

    private void Start()
    {
        // Try by tag first
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // Fallback — find by Health component if tag fails
        if (playerObj == null)
        {
            var health = FindFirstObjectByType<Player.Health>();
            if (health != null)
                playerObj = health.gameObject;
        }

        if (playerObj != null)
            _player = playerObj.transform;
        else
            Debug.LogWarning($"Npc ({name}): No Player found.");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (animator == null)
            animator = GetComponent<Animator>();

        SetDialoguePanelActive(false);
        SetPromptActive(false);
    }

    private void Update()
    {
        // Keep retrying to find player if not found yet
        if (_player == null)
        {
            Start();
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.position);
        _playerInRange = distance <= interactRange;

        // Show / hide interact prompt
        SetPromptActive(_playerInRange && !_isTalking);

        // Handle E key press
        if (_playerInRange && Input.GetKeyDown(interactKey))
        {
            if (_isTalking)
                AdvanceDialogue();
            else
                StartTalking();
        }

        // Close dialogue if player walks away
        if (_isTalking && !_playerInRange)
            StopTalking();

        // Smoothly look at player while talking
        if (_isTalking && lookAtPlayerWhenTalking)
            LookAtPlayer();

        // Play idle action animation when player is nearby but not talking
        if (_playerInRange && !_isTalking && animator != null)
            animator.SetTrigger(IdleActionHash);
    }

    // ── Dialogue Flow ─────────────────────────────────────────────

    private void StartTalking()
    {
        _isTalking = true;
        _dialogueIndex = 0;

        SetDialoguePanelActive(true);
        SetPromptActive(false);
        SetAnimatorTalking(true);
        PlaySound(greetSound);
        ShowLine(_dialogueIndex);
    }

    private void AdvanceDialogue()
    {
        // If still typing, skip to end of current line
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
            StopTalking();
    }

    private void StopTalking()
    {
        _isTalking = false;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        SetDialoguePanelActive(false);
        SetAnimatorTalking(false);
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

            if (talkSound != null && audioSource != null && c != ' ')
                audioSource.PlayOneShot(talkSound, 0.3f);

            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;
    }

    // ── Helpers ───────────────────────────────────────────────────

    private void LookAtPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void SetDialoguePanelActive(bool active)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(active);
    }

    private void SetPromptActive(bool active)
    {
        if (interactPromptText != null)
            interactPromptText.gameObject.SetActive(active);
    }

    private void SetAnimatorTalking(bool talking)
    {
        if (animator != null)
            animator.SetBool(IsTalkingHash, talking);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}