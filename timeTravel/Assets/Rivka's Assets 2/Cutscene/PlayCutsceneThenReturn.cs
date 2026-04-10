using UnityEngine;
using UnityEngine.Playables;

public class PlayCutsceneThenReturn : MonoBehaviour
{
    public PlayableDirector timeline;

    public GameObject player; // player root
    public GameObject playerCamera;
    public GameObject ThirdPersonCamera;

    public GameObject cutsceneCamera;    // main cutscene camera
    public GameObject cutsceneVFX;       // cutscene VFX
    public GameObject blackoutCamera;    // camera for flash/blackout
   
    
    void Start()
    {
        if (CharacterSelectionManager.runIntroCutscene) // only run if triggered
        {
            // Disable player & gameplay cameras
            player.SetActive(false);
            playerCamera.SetActive(false);
            ThirdPersonCamera.SetActive(false);

            // Enable cutscene camera & VFX
            if (cutsceneCamera != null)
                cutsceneCamera.SetActive(true);

            if (cutsceneVFX != null)
                cutsceneVFX.SetActive(true);

            // If using blackout at start, enable it here (optional)
            if (blackoutCamera != null)
                blackoutCamera.SetActive(false); // start off, timeline will activate it

            // Play timeline
            timeline.Play();
            timeline.stopped += OnTimelineEnd;

            // Reset the trigger so it doesn’t run next time
            CharacterSelectionManager.runIntroCutscene = false;
        }
        else
        {
            // Enable player/gameplay cameras immediately if no cutscene
            player.SetActive(true);
            playerCamera.SetActive(true);
            ThirdPersonCamera.SetActive(true);
        }
    }

    void OnTimelineEnd(PlayableDirector pd)
    {
        // Disable cutscene objects
        if (cutsceneCamera != null)
            cutsceneCamera.SetActive(false);

        if (cutsceneVFX != null)
            cutsceneVFX.SetActive(false);

        if (blackoutCamera != null)
            blackoutCamera.SetActive(false);

        // Give control back to player
        player.SetActive(true);
        playerCamera.SetActive(true);
        ThirdPersonCamera.SetActive(true);
    }
}