using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoorScript; // Assuming KeyScript is in the DoorScript namespace

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InventorySlot
    {
        public Image background; // Slot background
        public Image item; // Item image
        public KeyScript key; // Reference to the key associated with this slot
    }

    public List<InventorySlot> slots = new List<InventorySlot>(); // List of inventory slots
    public int currentSlot = 0; // Current active slot
    public Sprite activeSprite; // Active slot background sprite
    public Sprite inactiveSprite; // Inactive slot background sprite

    void Start()
    {
        // Initially disable all item images
        foreach (InventorySlot slot in slots)
        {
            slot.item.gameObject.SetActive(false);
        }
        UpdateSlotHighlight();
    }

    void Update()
    {
        // Switch between slots using the mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentSlot = (currentSlot + 1) % slots.Count;
            UpdateSlotHighlight();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentSlot = (currentSlot - 1 + slots.Count) % slots.Count;
            UpdateSlotHighlight();
        }

        // Drop current item when 'G' is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropCurrentItem();
        }
    }

    void UpdateSlotHighlight()
    {
        // Update highlighting of the current slot
        for (int i = 0; i < slots.Count; i++)
        {
            if (i == currentSlot)
            {
                slots[i].background.sprite = activeSprite; // Use active sprite
            }
            else
            {
                slots[i].background.sprite = inactiveSprite; // Use inactive sprite
            }
        }
    }

    public void AddItem(Sprite itemIcon, KeyScript key)
    {
        // Add item to the inventory
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].item.gameObject.activeSelf) // If item image is inactive
            {
                slots[i].item.sprite = itemIcon;
                slots[i].key = key;

                slots[i].item.gameObject.SetActive(true); // Enable item image display
                return;
            }
        }
    }

    public bool IsCurrentSlotKey(KeyScript requiredKey)
    {
        // Check if the current active slot contains the required key
        return slots[currentSlot].key == requiredKey && slots[currentSlot].item.gameObject.activeSelf;
    }

    public void RemoveCurrentItem()
    {
        // Remove current item from the inventory
        if (currentSlot >= 0 && currentSlot < slots.Count)
        {
            slots[currentSlot].item.gameObject.SetActive(false); // Disable item image
            slots[currentSlot].key = null; // Clear key reference
        }
    }

    public void DropCurrentItem()
{
    // Drop current item from the inventory
    if (currentSlot >= 0 && currentSlot < slots.Count && slots[currentSlot].key != null)
    {
        slots[currentSlot].key.DropKey(); // Activate the dropped key in the scene
        slots[currentSlot].item.gameObject.SetActive(false); // Disable item image
        slots[currentSlot].key = null; // Clear key reference
    }
}

}
