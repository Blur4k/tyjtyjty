using System.Collections;
using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        public bool open;
        public bool locked = true; // Флаг для обозначения, что дверь заблокирована
        public float smooth = 1.0f;
        public float startAngle = 0.0f; // Начальный угол открытия двери
        public float endAngle = 90.0f; // Конечный угол открытия двери
        private float currentAngle; // Текущий угол открытия двери
        public AudioSource audioSource;
        public AudioClip openDoor, closeDoor, tryOpenLockedDoor;
        public KeyScript requiredKey; // Ссылка на ключ, необходимый для открытия этой двери

        private InventoryManager inventoryManager; // Ссылка на InventoryManager
        private Coroutine shakeCoroutine; // Ссылка на запущенную корутину тряски

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            inventoryManager = FindObjectOfType<InventoryManager>(); // Получаем ссылку на InventoryManager
            currentAngle = startAngle; // Устанавливаем начальный угол открытия двери
        }

        void Update()
        {
            // Плавное открытие и закрытие двери
            var target = Quaternion.Euler(0, currentAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);
        }

        public void OpenDoor()
        {
            if (!locked) // Проверка, разблокирована ли дверь
            {
                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine); // Останавливаем корутину тряски
                    shakeCoroutine = null;
                }

                open = !open;
                audioSource.clip = open ? openDoor : closeDoor;
                audioSource.Play();

                if (open)
                {
                    currentAngle = endAngle; // Открываем дверь на конечный угол
                }
                else
                {
                    currentAngle = startAngle; // Закрываем дверь на начальный угол
                }
            }
        }

        public void TryToOpenDoor()
        {
            if (!locked || (inventoryManager != null && inventoryManager.IsCurrentSlotKey(requiredKey))) // Проверка, есть ли ключ в активном слоте или дверь уже разблокирована
            {
                locked = false; // Разблокировать дверь
                OpenDoor(); // Открыть дверь
                if (inventoryManager != null && inventoryManager.IsCurrentSlotKey(requiredKey))
                {
                    inventoryManager.RemoveCurrentItem(); // Удалить текущий предмет из инвентаря
                }
            }
            else
            {
                Debug.Log("Невозможно открыть дверь: ключ не в активном слоте или неверный ключ");
                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine); // Останавливаем предыдущую корутину тряски, если она существует
                }
                shakeCoroutine = StartCoroutine(ShakeDoor(0.5f, 0.8f, 10)); // Запускаем новую корутину тряски двери
                audioSource.clip = tryOpenLockedDoor;
                audioSource.Play();
            }
        }

        IEnumerator ShakeDoor(float intensity, float duration, int shakes)
        {
            float elapsedTime = 0f;
            Quaternion originalRotation = transform.localRotation;

            for (int i = 0; i < shakes; i++)
            {
                float shakeAmount = Random.Range(-5f, 5f) * intensity;
                Quaternion targetRotation = originalRotation * Quaternion.Euler(0f, shakeAmount, 0f);

                while (elapsedTime < duration / shakes)
                {
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10f);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                elapsedTime = 0f;
                transform.localRotation = originalRotation;

                yield return null;
            }

            shakeCoroutine = null; // Сбрасываем ссылку на корутину после завершения
        }
    }
}
