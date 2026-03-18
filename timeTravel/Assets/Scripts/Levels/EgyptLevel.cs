using UnityEngine;

public class EgyptLevel : Level
{
    [Header("Prefabs")]
    public GameObject npcPrefab;
    public GameObject dogEnemyPrefab;
    public GameObject staffPrefab;
    public GameObject puzzlePrefab;

    [Header("Spawn Points")]
    public Transform npcSpawnPoint;
    public Transform[] enemySpawnPoints;
    public Transform staffSpawnPoint;
    public Transform puzzleSpawnPoint;
    private bool puzzleSolved = false;
    private GameObject staffInstance;
    public override void Initialize()
    {
        Debug.Log("Egypt Level initialized.");

        SpawnNPCs();
        SpawnEnemies();
        SpawnPuzzle();
        SpawnStaff();
    }

    public override void SpawnNPCs()
    {
        // Instantiate NPC prefabs here
        Instantiate(npcPrefab, npcSpawnPoint.position, Quaternion.identity);
    }

    public override void SpawnEnemies()
    {
        // Instantiate guard dogs, etc.
        foreach (Transform spawn in enemySpawnPoints)
        {
            Instantiate(dogEnemyPrefab, spawn.position, Quaternion.identity);
        }
    }
    void SpawnPuzzle()
    {
        Instantiate(puzzlePrefab, puzzleSpawnPoint.position, Quaternion.identity);
    }
    void SpawnStaff()
    {
        staffInstance = Instantiate(staffPrefab, staffSpawnPoint.position, Quaternion.identity);
    }
    public override void StartLevel()
    {
        levelTimer.StartTimer(300f); // 5 minutes
        Debug.Log("Egypt Level started.");
    }

    public override void EndLevel()
    {
        Debug.Log("Egypt Level completed!");
        GameControl.Instance.LoadNextLevel();
    }
    public void OnPuzzleSolved()
    {
        puzzleSolved = true;

        Debug.Log("Final chamber unlocked!");
    }
}