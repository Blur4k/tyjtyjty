using UnityEngine;
using DoorScript;
using DrawerScript;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float distanceOpen = 3f; // Interaction distance
        public GameObject interactionImage; // UI element for interaction
        public AudioClip[] footstepSounds; // Array of footstep sounds
        public float footstepInterval = 0.5f; // Interval between footstep sounds

        private float lastFootstepTime; // Time of the last footstep sound
        private AudioSource audioSource; // AudioSource component for playing footstep sounds
        private bool isCrouched = false; // Track if the player is crouched

        void Start()
        {
            interactionImage.SetActive(false); // Initially hide the interaction UI

            // Initialize the AudioSource for footstep sounds
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        void Update()
        {
            // Handle crouch toggle
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                isCrouched = !isCrouched; // Toggle crouched state when Control is pressed
            }

            // Handle footstep sounds
            if (!isCrouched && IsPlayerMoving() && Time.time - lastFootstepTime > footstepInterval)
            {
                PlayFootstepSound();
                lastFootstepTime = Time.time;
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distanceOpen))
            {
                Door door = hit.transform.GetComponent<Door>();
                KeyScript key = hit.transform.GetComponent<KeyScript>();
                LightSwitch lightSwitch = hit.transform.GetComponent<LightSwitch>();
                Drawer drawer = hit.transform.GetComponent<Drawer>();

                if (drawer != null) // Проверка на ящик
                {
                    HandleDrawerInteraction(drawer);
                }
                else if (door != null) // Проверка на дверь
                {
                    HandleDoorInteraction(door);
                }
                else if (key != null) // Проверка на ключ
                {
                    HandleKeyInteraction(key);
                }
                else if (lightSwitch != null) // Проверка на переключатель
                {
                    HandleLightSwitchInteraction(lightSwitch);
                }
                else
                {
                    interactionImage.SetActive(false); // Disable interaction UI if not looking at an interactable object
                }
            }
            else
            {
                interactionImage.SetActive(false); // Disable interaction UI if nothing is hit by the raycast
            }
        }

        void HandleDrawerInteraction(Drawer drawer)
        {
            interactionImage.SetActive(true); // Enable interaction UI for the drawer

            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                drawer.ToggleDrawer(); // Open/Close the drawer
            }
        }

        void HandleDoorInteraction(Door door)
        {
            interactionImage.SetActive(true); // Enable interaction UI for the door

            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                door.TryToOpenDoor(); // Try to open the door
            }
        }

        void HandleKeyInteraction(KeyScript key)
        {
            interactionImage.SetActive(true); // Enable interaction UI for the key

            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                key.CollectKey(); // Collect the key
            }
        }

        void HandleLightSwitchInteraction(LightSwitch lightSwitch)
        {
            interactionImage.SetActive(true); // Enable interaction UI for the light switch

            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                lightSwitch.ToggleLights(); // Toggle the lights
            }
        }

        bool IsPlayerMoving()
        {
            // Example method to check if the player is moving (replace with your own movement logic)
            return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        }

        void PlayFootstepSound()
        {
            // Play a random footstep sound from the array
            if (footstepSounds.Length > 0)
            {
                AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
                audioSource.PlayOneShot(footstepSound);
            }
        }
    }
}
