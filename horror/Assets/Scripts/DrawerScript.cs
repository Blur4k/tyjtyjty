using System.Collections;
using UnityEngine;

namespace DrawerScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Drawer : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        public float openDistance = 0.5f; // Distance the drawer opens
        private Vector3 closedPosition; // Initial position of the drawer
        private Vector3 openPosition; // Target position when the drawer is open

        public AudioSource audioSource;
        public AudioClip openDrawer, closeDrawer;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            closedPosition = transform.localPosition;
            openPosition = closedPosition + new Vector3(0, 0, openDistance);
        }

        void Update()
        {
            // Smooth opening and closing of the drawer
            Vector3 targetPosition = open ? openPosition : closedPosition;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smooth);
        }

        public void ToggleDrawer()
        {
            open = !open; // Toggle drawer state
            audioSource.clip = open ? openDrawer : closeDrawer; // Set audio clip
            audioSource.Play(); // Play sound
        }
    }
}
