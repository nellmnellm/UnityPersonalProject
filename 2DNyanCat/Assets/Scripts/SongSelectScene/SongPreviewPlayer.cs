using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongPreviewPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = AudioManager.Instance.bgmGroup;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    public void PlayPreview(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopPreview()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}