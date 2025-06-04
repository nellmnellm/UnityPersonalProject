using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound;

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
            Debug.LogWarning("AudioManager not initialized. Click sound will use default output.");
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

}