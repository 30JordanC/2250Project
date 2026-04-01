using UnityEngine;

public class PlayerSelectionApplier : MonoBehaviour
{
    [Header("Assign your three player objects here")]
    public GameObject malePlayer;
    public GameObject middlePlayer;
    public GameObject femalePlayer;

    private void Start()
    {
        if (CharacterSelectionManager.Instance == null)
        {
            Debug.Log("CharacterSelectionManager not found. Defaulting to male.");
            SetOnlyOneActive(malePlayer);
            return;
        }

        switch (CharacterSelectionManager.Instance.selectedCharacter)
        {
            case CharacterSelectionManager.CharacterType.Male:
                SetOnlyOneActive(malePlayer);
                break;

            case CharacterSelectionManager.CharacterType.Middle:
                SetOnlyOneActive(middlePlayer);
                break;

            case CharacterSelectionManager.CharacterType.Female:
                SetOnlyOneActive(femalePlayer);
                break;

            default:
                SetOnlyOneActive(malePlayer);
                break;
        }
    }

    private void SetOnlyOneActive(GameObject activePlayer)
    {
        if (malePlayer != null)
            malePlayer.SetActive(activePlayer == malePlayer);

        if (middlePlayer != null)
            middlePlayer.SetActive(activePlayer == middlePlayer);

        if (femalePlayer != null)
            femalePlayer.SetActive(activePlayer == femalePlayer);
    }
}