using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    public enum CharacterType
    {
        None,
        Male,
        Middle,
        Female
    }

    public CharacterType selectedCharacter = CharacterType.None;

    [Header("Next Scene")]
    public string nextSceneName = "IntroScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectMale()
    {
        selectedCharacter = CharacterType.Male;
        Debug.Log("Male selected");
        LoadNextScene();
    }

    public void SelectMiddle()
    {
        selectedCharacter = CharacterType.Middle;
        Debug.Log("Middle character selected");
        LoadNextScene();
    }

    public void SelectFemale()
    {
        selectedCharacter = CharacterType.Female;
        Debug.Log("Female selected");
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("CharacterSelectionManager: nextSceneName is not set!");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}