using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public static bool runIntroCutscene = false; // signal to the intro scene
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
        runIntroCutscene = true; // <-- signal the intro cutscene to run
        LoadNextScene();
    }

    public void SelectMiddle()
    {
        selectedCharacter = CharacterType.Middle;
        Debug.Log("Middle character selected");
        runIntroCutscene = true; // <-- signal the intro cutscene to run
        LoadNextScene();
    }

    public void SelectFemale()
    {
        selectedCharacter = CharacterType.Female;
        Debug.Log("Female selected");
        runIntroCutscene = true; // <-- signal the intro cutscene to run
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