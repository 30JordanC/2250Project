using UnityEngine;

namespace Level6Scripts
{


    public class TestCamera : MonoBehaviour
    {
        private void Start()
        {
            // Disable this camera if a player camera exists
            // so it doesn't conflict with the player's camera
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                GetComponent<Camera>().enabled = false;
            }
        }
    }
}