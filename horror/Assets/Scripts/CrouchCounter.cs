using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CrouchCounter : MonoBehaviour
{
    public AudioClip[] crouchSounds;
    public AudioClip startSound; // Single sound for start
    public AudioSource audioSource;  // Main AudioSource for playing sounds
    public AudioClip thirtyCrouchSound; // Sound for 30th crouch
    public float minCrouchInterval = 0.5f; // Minimum interval between crouches
    public string nextSceneName = "NextScene"; // Name of the next scene

    private int crouchCount = 0;
    private float lastCrouchTime = 0f;
    private AudioClip lastCrouchClip; // To store the last played crouch sound
    private int controlPressCount = 0; // Track number of Control key presses

    void Start()
    {
        audioSource.PlayOneShot(startSound);
    }

    void Update()    
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controlPressCount++;
            float currentTime = Time.time;

            if (controlPressCount == 1)
            {
                lastCrouchTime = currentTime;
            }
            else if (controlPressCount == 2)
            {
                if (currentTime - lastCrouchTime >= minCrouchInterval)
                {
                    crouchCount++;
                    PlayCrouchSound(crouchCount);

                    if (crouchCount == 20) // Assuming 20th crouch is the goal
                    {
                        StartCoroutine(PlaySoundAndLoadScene(thirtyCrouchSound, nextSceneName));
                    }
                }
                else
                {
                    // Play the last crouch sound again, or a default sound if lastCrouchClip is null
                    audioSource.PlayOneShot(lastCrouchClip ?? crouchSounds[0]);
                }

                controlPressCount = 0; // Reset after counting two presses
                lastCrouchTime = 0f;
            }
        }
    }

    void PlayCrouchSound(int count)
    {
        if (count > 0 && count <= crouchSounds.Length)
        {
            lastCrouchClip = crouchSounds[count - 1];
            audioSource.PlayOneShot(lastCrouchClip);
        }
    }

    IEnumerator PlaySoundAndLoadScene(AudioClip clip, string sceneName)
    {
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        SceneManager.LoadScene(sceneName);
    }
}
