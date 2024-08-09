using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Light[] controlledLights; // Массив источников света
    public AudioClip switchOnSound; // Звук при включении
    public AudioClip switchOffSound; // Звук при выключении

    private AudioSource audioSource; // Компонент AudioSource

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Выключаем все источники света в начале игры
        foreach (Light light in controlledLights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }

    public void ToggleLights()
    {
        bool anyLightEnabled = false;

        foreach (Light light in controlledLights)
        {
            if (light != null)
            {
                light.enabled = !light.enabled;
                if (light.enabled)
                {
                    anyLightEnabled = true;
                }
            }
        }

        // Воспроизводим соответствующий звук
        if (anyLightEnabled)
        {
            PlaySound(switchOnSound);
        }
        else
        {
            PlaySound(switchOffSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
