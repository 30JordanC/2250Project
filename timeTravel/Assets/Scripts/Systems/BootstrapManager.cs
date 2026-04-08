using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneTransitionManager.Instance.LoadScene("IntroScene", "IntroSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
