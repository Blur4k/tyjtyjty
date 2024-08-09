using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public LapTracker lapTracker; // Ссылка на LapTracker

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lapTracker.CheckpointPassed(gameObject);
        }
    }
}
