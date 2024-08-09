using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(Collider))] // Ensure a collider is present for interaction
    public class KeyScript : MonoBehaviour
    {
        [SerializeField] Sprite keyIcon; // Icon of the key for the inventory
        public Door doorToOpen; // Reference to the door that this key can open
        private bool isCollected = false; // Flag to track if the key is collected
        private Transform originalParent; // Original parent transform to restore when dropping

        void Start()
        {
            originalParent = transform.parent; // Save the original parent transform
        }

        void Update()
        {
            if (!isCollected)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        if (Input.GetMouseButtonDown(0)) // Left mouse button
                        {
                            CollectKey(); // Call CollectKey on interaction
                        }
                    }
                }
            }

            // Check for dropping the key
            if (isCollected && Input.GetKeyDown(KeyCode.G))
            {
                DropKey(); // Call DropKey when the G key is pressed
            }
        }

        public void CollectKey()
        {
            if (doorToOpen != null)
            {
                InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
                if (inventoryManager != null)
                {
                    inventoryManager.AddItem(keyIcon, this); // Pass 'this' as the KeyScript object
                }
                gameObject.SetActive(false); // Deactivate the key object
                isCollected = true; // Mark the key as collected
            }
            else
            {
                Debug.LogError("No door assigned for the key: " + gameObject.name);
            }
        }

        public void DropKey()
        {
            // Set the position of the key to be near the player's feet
            Vector3 dropPosition = Camera.main.transform.position + Camera.main.transform.forward * 1.5f; // Example: Drop 1.5 units in front of the camera
            transform.position = new Vector3(dropPosition.x, 0.01f, dropPosition.z); // Ensure the key drops at ground level
            transform.parent = originalParent; // Restore the original parent
            gameObject.SetActive(true); // Activate the key object
            isCollected = false; // Mark the key as not collected
        }
    }
}
