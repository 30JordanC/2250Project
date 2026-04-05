using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Npc : MonoBehaviour
{
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
    public float interactRange = 5f;
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

    private Transform _player;
    private int _dialogueIndex;
    private bool _isTalking;
    private bool _playerInRange;
    private bool _isTyping;
    private Coroutine _typingCoroutine;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SetDialoguePanelActive(false);
        SetPromptActive(false);
    }

    private void Update()
    {
        if (_player == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.position);

        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"NPC Distance: {distance} | InRange: {_playerInRange} | Talking: {_isTalking} | Panel: {dialoguePanel != null} | Text: {dialogueText != null} | Prompt: {interactPromptText != null}");
        }

        _playerInRange = distance <= interactRange;

        SetPromptActive(_playerInRange && !_isTalking);

        if (_playerInRange && Input.GetKeyDown(interactKey))
        {
            if (_isTalking)
                AdvanceDialogue();
            else
                StartTalking();
        }

        if (_isTalking && !_playerInRange)
            StopTalking();

        if (_isTalking && lookAtPlayerWhenTalking)
            LookAtPlayer();
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
        {
            foreach (GameObject obj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (obj.name == "PlayerRoot" || obj.name == "Player")
                {
                    playerObj = obj;
                    Debug.Log($"Npc: Found player by name: {obj.name}");
                    break;
                }
            }
        }

        if (playerObj != null)
        {
            _player = playerObj.transform;
            Debug.Log($"Npc: Player assigned: {playerObj.name} | Tag: {playerObj.tag}");
        }
        else
        {
            if (Time.frameCount % 120 == 0)
                Debug.LogWarning("Npc: Player not found yet, still searching...");
        }
    }

    private void StartTalking()
    {
        _isTalking = true;
        _dialogueIndex = 0;
        SetDialoguePanelActive(true);
        SetPromptActive(false);
        PlaySound(greetSound);
        ShowLine(_dialogueIndex);
        Debug.Log("Npc: StartTalking called!");
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
            StopTalking();
    }

    private void StopTalking()
    {
        _isTalking = false;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        SetDialoguePanelActive(false);
        SetPromptActive(false);
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
        else
            Debug.LogWarning("Npc: DialoguePanel is not assigned in Inspector!");
    }

    private void SetPromptActive(bool active)
    {
        if (interactPromptText != null)
            interactPromptText.gameObject.SetActive(active);
        else
            Debug.LogWarning("Npc: InteractPromptText is not assigned in Inspector!");
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