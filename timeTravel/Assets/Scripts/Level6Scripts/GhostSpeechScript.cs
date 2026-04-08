using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Level6Scripts
{
    public class GhostSpeech : MonoBehaviour
    {
        [Header("Dialogue")]
        [TextArea(2, 5)]
        public string[] speechLines = new string[]
        {
            "Ha ha ha! You dare approach me?",
            "I am just a puppet... a mere shadow!",
            "The REAL demon lives within the Terra Artifact!",
            "Touch it... and FREE this world from the demon's curse!",
            "If you dare..."
        };

        [Header("UI")]
        public GameObject speechPanel;
        public Text speechText;
        public float typingSpeed = 0.05f;
        public float autoAdvanceDelay = 2.5f;
        public float triggerRange = 5f;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip laughSound;
        public AudioClip[] voiceLines;

        private bool _hasSpeaked = false;
        private bool _isSpeaking = false;
        private int _lineIndex = 0;
        private Coroutine _typingCoroutine;
        private Coroutine _advanceCoroutine;

        private void Start()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            SetPanelActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasSpeaked) return;
            if (other.CompareTag("Player") || other.name.Contains("Player"))
            {
                StartSpeech();
            }
        }

        private void StartSpeech()
        {
            _hasSpeaked = true;
            _isSpeaking = true;
            _lineIndex = 0;

            SetPanelActive(true);

            // Play laugh sound first
            if (laughSound != null && audioSource != null)
                audioSource.PlayOneShot(laughSound);

            ShowLine(_lineIndex);
            Debug.Log("GhostSpeech: Ghost started speaking!");
        }

        private void ShowLine(int index)
        {
            if (index >= speechLines.Length)
            {
                StopSpeech();
                return;
            }

            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            if (_advanceCoroutine != null)
                StopCoroutine(_advanceCoroutine);

            if (voiceLines != null && index < voiceLines.Length && voiceLines[index] != null)
            {
                audioSource.clip = voiceLines[index];
                audioSource.Play();
            }

            _typingCoroutine = StartCoroutine(TypeLine(speechLines[index]));
        }

        private IEnumerator TypeLine(string line)
        {
            if (speechText != null)
                speechText.text = "";

            foreach (char c in line)
            {
                if (speechText != null)
                    speechText.text += c;

                yield return new WaitForSeconds(typingSpeed);
            }

            _advanceCoroutine = StartCoroutine(AutoAdvance());
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

            _lineIndex++;
            ShowLine(_lineIndex);
        }

        private void StopSpeech()
        {
            _isSpeaking = false;

            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            if (_advanceCoroutine != null)
                StopCoroutine(_advanceCoroutine);

            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();

            SetPanelActive(false);
            Debug.Log("GhostSpeech: Ghost finished speaking!");
        }

        private void SetPanelActive(bool active)
        {
            if (speechPanel != null)
                speechPanel.SetActive(active);
            else
                Debug.LogWarning("GhostSpeech: speechPanel not assigned!");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRange);
        }
    }
}