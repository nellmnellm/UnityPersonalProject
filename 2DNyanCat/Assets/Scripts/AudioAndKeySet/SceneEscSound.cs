using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SceneEscSound : MonoBehaviour
{
    public AudioClip escSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (AudioManager.Instance != null)
        {
            audioSource.outputAudioMixerGroup = AudioManager.Instance.sfxGroup;
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null, using default audio output.");
        }

    }

    public void PlayEscSound()
    {
        if (escSound != null)
        {
            audioSource.PlayOneShot(escSound);
        }
        else
        {
            Debug.LogWarning("escSound not assigned in SceneEscSound.");
        }
    }
}