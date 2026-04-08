using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{
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

            SphereCollider trigger = GetComponent<SphereCollider>();
            if (trigger == null)
            {
                trigger = gameObject.AddComponent<SphereCollider>();
                trigger.radius = 3f;
                trigger.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy")) return;
            if (other.gameObject == gameObject) return;
            if (NpcManager.AnyNpcTalking) return;

            if (!_isTalking)
            {
                _player = other.transform;
                StartTalking();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_player != null && other.transform == _player)
            {
                StopTalking();
                _player = null;
                _dialogueIndex = 0;
            }
        }

        private void Update()
        {
            // Empty — no rotation
        }

        private void StartTalking()
        {
            NpcManager.AnyNpcTalking = true;
            _isTalking = true;
            _dialogueIndex = 0;
            SetDialoguePanelActive(true);

            if (greetSound != null)
                PlaySound(greetSound);

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
            NpcManager.AnyNpcTalking = false;
            _isTalking = false;
            _dialogueIndex = 0;

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

            if (voiceLines != null && index < voiceLines.Length && voiceLines[index] != null)
            {
                audioSource.clip = voiceLines[index];
                audioSource.Play();
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
            Gizmos.DrawWireSphere(transform.position, 3f);
        }
    }
}