using UnityEngine;

public class PuzzleInteraction : MonoBehaviour
{
    public Puzzle puzzle;

    void OnMouseDown()
    {
        puzzle.SolvePuzzle();
    }
}