using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TitleBGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip;

    private AudioSource audioSource;


    IEnumerator Start()
    {
        yield return null;
        audioSource = GetComponent<AudioSource>();
        if (AudioManager.Instance != null)
        {
            audioSource.outputAudioMixerGroup = AudioManager.Instance.bgmGroup;
        }
        else
        {
            Debug.LogWarning("AudioManager not initialized. Click sound will use default output.");
        }
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        PlayBGMSound();
    }

    public void PlayBGMSound()
    {
        if (bgmClip != null)
            audioSource.PlayOneShot(bgmClip);
    }
}