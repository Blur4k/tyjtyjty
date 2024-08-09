using UnityEngine;

public class TriggerPanelActivation : MonoBehaviour
{
    public GameObject panel; // Панель, которую нужно активировать

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Проверка, что триггер касается игрока
        {
            ActivatePanelAndStopTime();
        }
    }

    private void ActivatePanelAndStopTime()
    {
        if (panel != null)
        {
            panel.SetActive(true); // Активируем панель
        }

        Time.timeScale = 0f; // Останавливаем время
    }
}
