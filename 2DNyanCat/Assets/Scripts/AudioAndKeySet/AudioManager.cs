using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixer audioMixer; // 연결: MainAudioMixer
    public AudioMixerGroup inGameMusicGroup; //연결 : GameManager에서 활성화하는 audioSource에 할당해줄것임.
    public AudioMixerGroup bgmGroup;         //아직 오디오 파일이 없음
    public AudioMixerGroup sfxGroup;         //추가 예정.
    [Range(0f, 1f)] public float masterVolume = 0.3f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;
    [Range(0f, 1f)] public float inGameVolume = 0.5f;
    //토글용 변수와 마지막 볼륨 저장용 Dictionary;
    private bool isMuted = false;
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

    public void ToggleMute()
    {
        isMuted = !isMuted;

        string[] parameters = { "Master", "BGM", "SFX", "InGame" };

        foreach (string param in parameters)
        {
            if (isMuted)
            {
                // 현재 볼륨 저장 후 음소거
                audioMixer.GetFloat(param, out float currentDb);
                lastVolumes[param] = currentDb;
                audioMixer.SetFloat(param, -80f); // 최소값 (거의 무음)
            }
            else
            {
                // 복원
                if (lastVolumes.TryGetValue(param, out float prevDb))
                    audioMixer.SetFloat(param, prevDb);
                else
                    audioMixer.SetFloat(param, 0f); // fallback
            }
        }
    }
}