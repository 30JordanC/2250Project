using UnityEngine;

namespace Level6Scripts
{
    public class Level6Manager : MonoBehaviour
    {
        public static Level6Manager Instance;

        public bool hasSword;
        public bool bossDead;
        public bool levelComplete;

        // Assign the terra pickup object in Inspector — it starts disabled
        // and gets enabled when the boss dies
        public GameObject terraObject;

        private void Awake()
        {
            // FIX: Added singleton guard. Original would overwrite Instance
            // if somehow two managers existed, causing unexpected behaviour.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Level6Manager: Duplicate instance found and destroyed.");
                Destroy(gameObject);
            }
        }

        public void CollectSword()
        {
            hasSword = true;
            Debug.Log("Level6Manager: Sword collected. Press F to attack.");
        }

        public void BossDefeated()
        {
            bossDead = true;
            Debug.Log("Level6Manager: Boss defeated! Terra pickup is now available.");

            if (terraObject != null)
                terraObject.SetActive(true);
            else
                Debug.LogWarning("Level6Manager: terraObject is not assigned in Inspector.");
        }

        public void CompleteLevel()
        {
            if (levelComplete) return;
            levelComplete = true;
            Debug.Log("Level6Manager: Level 6 Complete!");

            // Hook your scene transition here, for example:
            // SceneTransitionManager.Instance.LoadScene("Credits", "SpawnStart");
        }
    }
}