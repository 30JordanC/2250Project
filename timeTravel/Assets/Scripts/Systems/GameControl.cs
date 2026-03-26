using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public Level currentLevel;
    //public Player player;
    public int levelIndex = 0;
    public Level[] allLevels;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int index)
    {
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);

        Level next = Instantiate(allLevels[index]);
        currentLevel = next;
        levelIndex = index;

        currentLevel.Initialize();
        currentLevel.StartLevel();
    }

    public void LoadNextLevel()
    {
        if (levelIndex + 1 < allLevels.Length)
            LoadLevel(levelIndex + 1);
        else
            CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        Debug.Log("All artifacts collected. GAME COMPLETE.");
    }

    public void CheckLoseCondition()
    {
        Debug.Log("Player lost the level.");
        LoadLevel(levelIndex);
    }
}