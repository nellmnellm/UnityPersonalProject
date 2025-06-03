using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixer audioMixer; // ����: MainAudioMixer
    public AudioMixerGroup inGameMusicGroup; //���� : GameManager���� Ȱ��ȭ�ϴ� audioSource�� �Ҵ����ٰ���.
    public AudioMixerGroup bgmGroup;         //���� ����� ������ ����
    public AudioMixerGroup sfxGroup;         //�߰� ����.
    [Range(0f, 1f)] public float masterVolume = 0.3f;
    [Range(0f, 1f)] public float bgmVolume = 0.3f;
    [Range(0f, 1f)] public float sfxVolume = 0.3f;
    [Range(0f, 1f)] public float inGameVolume = 0.3f;
    //������ ���� ����� Dictionary;
    private Dictionary<string, float> lastVolumes = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void SetVolume(string parameter, float normalizedValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(normalizedValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);

        switch (parameter)
        {
            case "Master":
                masterVolume = normalizedValue;
                break;
            case "BGM":
                bgmVolume = normalizedValue;
                break;
            case "SFX":
                sfxVolume = normalizedValue;
                break;
            case "InGame":
                inGameVolume = normalizedValue;
                break;
        }
    }

    public void ToggleMuteSingle(string paramName)
    {
        bool currentlyMuted = audioMixer.GetFloat(paramName, out float currentDb) && currentDb <= -79f;

        if (currentlyMuted)
        {
            float restoredDb = lastVolumes.TryGetValue(paramName, out float prevDb) ? prevDb : 0f;
            audioMixer.SetFloat(paramName, restoredDb);
        }
        else
        {
            audioMixer.GetFloat(paramName, out currentDb);
            lastVolumes[paramName] = currentDb;
            audioMixer.SetFloat(paramName, -80f);
        }
    }
}