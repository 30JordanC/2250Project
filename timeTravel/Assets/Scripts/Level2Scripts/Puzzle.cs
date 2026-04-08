using TMPro;
using UnityEngine;

namespace Level2Scripts
{
    public class Puzzle : MonoBehaviour
    {
        public bool isSolved = false;
        public GameObject puzzleUI;
        public TMP_InputField answerInput;
        public int correctAnswer = 8;
        public Door door;
    
        public void SolvePuzzle()
        {
            isSolved = true;
            Debug.Log("Puzzle solved!");

            EgyptLevel level = FindObjectOfType<EgyptLevel>();
            if (level != null)
            {
                level.OnPuzzleSolved();
            }

            if (door != null)
            {
                door.OpenDoor();
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            puzzleUI.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player touched the cube!");

                gameObject.SetActive(false);
                puzzleUI.SetActive(true);
                //unlocking cursor 
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        public void CheckAnswer()
        {
            int playerAnswer;

            if (int.TryParse(answerInput.text, out playerAnswer))
            {
                if (playerAnswer == correctAnswer)
                {
                    SolvePuzzle();
                }
                else
                {
                    Debug.Log("Wrong answer!");
                }
            }
            else
            {
                Debug.Log("Invalid input!");
            }
        }
    }
}