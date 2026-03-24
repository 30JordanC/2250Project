using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public bool isSolved = false;
    public GameObject puzzleUI;

    public void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("Puzzle solved!");

        EgyptLevel level = FindObjectOfType<EgyptLevel>();
        if (level != null)
        {
            level.OnPuzzleSolved();
        }

        puzzleUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the cube!");

            gameObject.SetActive(false);
            puzzleUI.SetActive(true);
        }
    }
}