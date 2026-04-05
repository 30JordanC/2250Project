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

    [Header("Voice Lines - one clip per dialogue line")]
    public AudioClip[] voiceLines;

    [Header("UI - Assign in Inspector")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public float typingSpeed = 0.04f;
    public float autoAdvanceDelay = 3f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip greetSound;

    private Transform _player;
    private int _dialogueIndex;
    private bool _isTalking;
    private bool _isTyping;
    private Coroutine _typingCoroutine;
    private Coroutine _autoAdvanceCoroutine;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        SetDialoguePanelActive(false);

        // Auto add sphere collider trigger if missing
        SphereCollider trigger = GetComponent<SphereCollider>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<SphereCollider>();
            trigger.radius = 5f;
            trigger.isTrigger = true;
            Debug.Log("Npc: Added SphereCollider trigger automatically!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) return;
        if (other.gameObject == gameObject) return;

        if (!_isTalking)
        {
            _player = other.transform;
            Debug.Log($"Npc: Triggered by {other.gameObject.name}");
            StartTalking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player != null && other.transform == _player)
        {
            StopTalking();
            _player = null;
        }
    }

    private void Update()
    {
        // Rotation removed — Golem stays in place
    }

    private void StartTalking()
    {
        _isTalking = true;
        _dialogueIndex = 0;

        SetDialoguePanelActive(true);

        if (greetSound != null)
            PlaySound(greetSound);

        ShowLine(_dialogueIndex);
        Debug.Log("Npc: StartTalking!");
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

            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();

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

        if (_autoAdvanceCoroutine != null)
            StopCoroutine(_autoAdvanceCoroutine);

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        SetDialoguePanelActive(false);
    }

    private void ShowLine(int index)
    {
        if (index >= dialogueLines.Length) return;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        if (_autoAdvanceCoroutine != null)
            StopCoroutine(_autoAdvanceCoroutine);

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        // Play matching voice clip
        if (voiceLines != null && index < voiceLines.Length && voiceLines[index] != null)
        {
            audioSource.clip = voiceLines[index];
            audioSource.Play();
            Debug.Log($"Npc: Playing voice line {index}");
        }

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

        _autoAdvanceCoroutine = StartCoroutine(AutoAdvance());
    }

    private IEnumerator AutoAdvance()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            while (audioSource.isPlaying)
                yield return null;
        }
        else
        {
            yield return new WaitForSeconds(autoAdvanceDelay);
        }

        AdvanceDialogue();
    }

    private void SetDialoguePanelActive(bool active)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(active);
        else
            Debug.LogWarning("Npc: DialoguePanel not assigned!");
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}