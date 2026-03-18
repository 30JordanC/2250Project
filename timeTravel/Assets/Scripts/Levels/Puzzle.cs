using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public bool isSolved = false;

    public void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("Puzzle solved!");

        // Notify level
        FindObjectOfType<EgyptLevel>().OnPuzzleSolved();
    }
}