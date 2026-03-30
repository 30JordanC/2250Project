using UnityEngine;

namespace Level6Scripts
{
    public class Level6Manager : MonoBehaviour
    {
        public static Level6Manager Instance;

        public bool hasSword;
        public bool bossDead;
        public bool levelComplete;

        public GameObject terraObject;

        private void Awake()
        {
            Instance = this;
        }

        public void CollectSword()
        {
            hasSword = true;
            Debug.Log("Sword collected");
        }

        public void BossDefeated()
        {
            bossDead = true;
            Debug.Log("Boss defeated");

            if (terraObject != null)
            {
                terraObject.SetActive(true);
            }
        }

        public void CompleteLevel()
        {
            levelComplete = true;
            Debug.Log("Level 6 Complete");
        }
    }
}