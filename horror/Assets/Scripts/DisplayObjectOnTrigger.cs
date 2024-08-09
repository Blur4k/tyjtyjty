using UnityEngine;

public class DisplayObjectOnTrigger : MonoBehaviour
{
    public GameObject player;                  // Игрок
    public GameObject triggerObject;           // Объект-триггер
    public GameObject displayObjectPrefab;     // Префаб объекта, который будет отображаться перед игроком
    private GameObject displayObjectInstance;  // Экземпляр отображаемого объекта

    private bool isPlayerOnTrigger = false;

    void Update()
    {
        // Проверяем, стоит ли игрок на объекте-триггере
        if (isPlayerOnTrigger)
        {
            if (displayObjectInstance == null)
            {
                // Создаём объект перед лицом игрока, если его ещё нет
                displayObjectInstance = Instantiate(displayObjectPrefab, player.transform);
                displayObjectInstance.transform.localPosition = new Vector3(0, 0, 2); // Позиция перед лицом игрока
            }
            else
            {
                // Обновляем позицию объекта перед лицом игрока
                displayObjectInstance.transform.localPosition = new Vector3(0, 0, 2);
            }
        }
        else
        {
            // Удаляем объект, если игрок не на триггере
            if (displayObjectInstance != null)
            {
                Destroy(displayObjectInstance);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerOnTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerOnTrigger = false;
        }
    }
}
