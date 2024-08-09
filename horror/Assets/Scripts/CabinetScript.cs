using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Cabinet : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        public Vector3 openRotation = new Vector3(0, -90, 0); // Полный угол открытия шкафа
        public Vector3 initialRotation = Vector3.zero; // Начальный угол открытия шкафа
        private Vector3 currentRotation; // Текущий угол открытия шкафа

        public AudioSource audioSource;
        public AudioClip openCabinet, closeCabinet;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentRotation = initialRotation;
            transform.localRotation = Quaternion.Euler(initialRotation);
        }

        void Update()
        {
            // Плавное открытие и закрытие шкафа
            var target = Quaternion.Euler(currentRotation);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);

            // Check if the cursor is over the cabinet and if the left mouse button is pressed
            if (IsCursorOverObject() && Input.GetMouseButtonDown(0))
            {
                ToggleCabinet();
            }
        }

        private bool IsCursorOverObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform;
            }
            return false;
        }

        public void ToggleCabinet()
        {
            open = !open;
            audioSource.clip = open ? openCabinet : closeCabinet;
            audioSource.Play();

            currentRotation = open ? openRotation : initialRotation;
        }
    }
}
