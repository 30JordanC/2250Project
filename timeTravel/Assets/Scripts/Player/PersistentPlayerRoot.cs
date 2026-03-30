using UnityEngine;

public class PersistentPlayerRoot : MonoBehaviour
{
    private static PersistentPlayerRoot instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}