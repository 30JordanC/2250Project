using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public GameObject playerRoot;

    private string targetSpawnID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, string spawnID)
    {
        targetSpawnID = spawnID;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(MovePlayerToSpawn());
    }

    private IEnumerator MovePlayerToSpawn()
    {
        yield return null;

        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint targetSpawn = null;

        foreach (SpawnPoint spawn in spawnPoints)
        {
            if (spawn.spawnID == targetSpawnID)
            {
                targetSpawn = spawn;
                break;
            }
        }

        if (targetSpawn == null && spawnPoints.Length > 0)
        {
            targetSpawn = spawnPoints[0];
        }

        if (targetSpawn != null && playerRoot != null)
        {
            Rigidbody rb = playerRoot.GetComponentInChildren<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            playerRoot.transform.position = targetSpawn.transform.position;
            playerRoot.transform.rotation = targetSpawn.transform.rotation;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}