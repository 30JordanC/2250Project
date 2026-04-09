using UnityEngine; //to reset the puzzle

public class RuneReset : MonoBehaviour
{
    void Start()
    {
        RunePuzzle.activatedCount = 0;
        Debug.Log("Rune count reset!");
    }
}