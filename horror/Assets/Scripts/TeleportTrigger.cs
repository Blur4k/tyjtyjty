using System.Collections;
using UnityEngine;

namespace PlayerMovement
{
    public class TeleportTrigger : MonoBehaviour
    {
        public Transform targetPoint; // Целевая точка для перемещения
        public float smooth = 1.0f; // Скорость плавного перемещения

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Проверка, что триггер касается игрока
            {
                StartCoroutine(MovePlayer(other));
            }
        }

        private IEnumerator MovePlayer(Collider playerCollider)
        {
            Transform player = playerCollider.transform;
            Vector3 startPosition = player.position;
            Vector3 endPosition = targetPoint.position;
            float elapsedTime = 0f;

            // Отключаем коллайдеры игрока
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();
            foreach (Collider col in playerColliders)
            {
                col.enabled = false;
            }

            while (elapsedTime < smooth)
            {
                player.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / smooth);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            player.position = endPosition; // Устанавливаем точную конечную позицию

            // Включаем коллайдеры игрока
            foreach (Collider col in playerColliders)
            {
                col.enabled = true;
            }
        }
    }
}
