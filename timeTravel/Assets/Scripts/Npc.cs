using UnityEngine;
using UnityEngine.UI;
using System.Collections;


    /// <summary>
    /// Full Npc system with:
    /// - Proximity detection
    /// - E key interaction prompt
    /// - Multi-line dialogue cycling
    /// - Optional quest trigger (gives sword hint)
    /// - Idle look-at-player behaviour
    /// - Works with or without an Animator
    /// </summary>
    public class Npc : MonoBehaviour
    {
        // ── Inspector ────────────────────────────────────────────────
        [Header("Dialogue")]
        [TextArea(2, 5)]
        public string[] dialogueLines = new string[]
        {
            "Traveller! The golem guards this land.",
            "Find the Legendary Axe before you face it.",
            "Strike it down and the Terra stone will appear.",
            "Bring me the Terra stone to complete your journey!"
        };

        [Header("Interaction")]
        public float interactRange = 3f;
        public KeyCode interactKey = KeyCode.E;

        [Header("UI - Assign in Inspector")]
        public GameObject dialoguePanel;       // A UI panel (e.g. Image background)
        public Text dialogueText;              // Text inside the panel
        public Text interactPromptText;        // Small "Press E" prompt
        public float typingSpeed = 0.04f;      // Typewriter speed per character

        [Header("Npc Behaviour")]
        public bool lookAtPlayerWhenTalking = true;
        public float rotationSpeed = 4f;

        [Header("Sounds")]
        public AudioSource audioSource;
        public AudioClip talkSound;            // Short blip per character (optional)
        public AudioClip greetSound;           // Plays when player enters range

        [Header("Optional Animator")]
        public Animator animator;
        // Animator bool name to set true while talking
        public string talkingBoolName = "isTalking";

        // ── Private ──────────────────────────────────────────────────
        private Transform _player;
        private int _dialogueIndex;
        private bool _isTalking;
        private bool _playerInRange;
        private bool _isTyping;
        private Coroutine _typingCoroutine;

        private static readonly int IsTalkingHash = Animator.StringToHash("isTalking");

        // ─────────────────────────────────────────────────────────────

        private void Start()
        {
            // Find player by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                _player = playerObj.transform;
            else
                Debug.LogWarning($"Npc ({name}): No GameObject tagged 'Player' found.");

            // Auto-grab AudioSource if not assigned
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            // Auto-grab Animator if not assigned
            if (animator == null)
                animator = GetComponent<Animator>();

            // Hide UI on start
            SetDialoguePanelActive(false);
            SetPromptActive(false);
        }

        private void Update()
        {
            if (_player == null) return;

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

            // Close dialogue if player walks away mid-conversation
            if (_isTalking && !_playerInRange)
                StopTalking();

            // Smoothly look at player while talking
            if (_isTalking && lookAtPlayerWhenTalking)
                LookAtPlayer();
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
            {
                ShowLine(_dialogueIndex);
            }
            else
            {
                StopTalking();
            }
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

                // Play a soft blip every few characters
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

        // Draw interact range in Scene view
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }
    }
