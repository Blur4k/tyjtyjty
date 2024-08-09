using UnityEngine;
using UnityEngine.SceneManagement;

public class LapTracker : MonoBehaviour
{
    public string sceneToLoad;  // Имя сцены, которую нужно загрузить после завершения гонки

    private int totalCheckpoints;
    private int checkpointsPassed;

    private void Start()
    {
        // Находим все контрольные точки в сцене
        totalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
        checkpointsPassed = 0;
        Debug.Log($"Найдено контрольных точек: {totalCheckpoints}");
    }

    public void CheckpointPassed(GameObject checkpoint)
    {
        checkpointsPassed++;
        Debug.Log($"Контрольная точка пересечена: {checkpoint.name}");
        Debug.Log($"Количество пройденных контрольных точек: {checkpointsPassed}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartLine"))
        {
            // Запускаем отслеживание, если пересечена стартовая линия
            Debug.Log("Стартовая линия пересечена");
        }
        else if (other.CompareTag("FinishLine"))
        {
            // Проверяем, прошёл ли игрок все контрольные точки
            Debug.Log("Проверка финишной линии:");
            Debug.Log($"Количество контрольных точек: {totalCheckpoints}");
            Debug.Log($"Количество пройденных контрольных точек: {checkpointsPassed}");

            if (checkpointsPassed == totalCheckpoints)
            {
                Debug.Log("Финишная линия засчитана!");
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Финишная линия не засчитана: не все контрольные точки пройдены.");
            }
        }
    }
}
