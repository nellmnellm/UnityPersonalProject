using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.Audio;

//사운드 구분을 위한 Enum 설계
public enum BGM
{
    Title, Stage1Field, Stage1Boss,
}

public enum SFX
{
    Bullet, 
}
[Serializable]
public class BGMClip
{
    public BGM type;
    public AudioClip clip;
}
public class SFXClip
{
    public SFX type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM"; //오디오 믹서의 이름 => 귀찮으니 변수화
    public string sfxParameter = "SFX";

    [Header("오디오 소스")]
    public AudioSource bgm;
    public AudioSource sfx;

    [Header("오디오 클립")]
    public List<BGMClip> bgm_list;
    public List<SFXClip> sfx_list;

    private Dictionary<BGM, AudioClip> bgm_dict;
    private Dictionary<SFX, AudioClip> sfx_dict;
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //딕셔너리 생성
            bgm_dict = new Dictionary<BGM, AudioClip>();
            sfx_dict = new Dictionary<SFX, AudioClip>();
            //리스트에 있는 데이터들 딕셔너리에 등록.
            foreach(var bgm in bgm_list)
            {
                bgm_dict[bgm.type] = bgm.clip;
            }

            foreach(var sfx in sfx_list)
            {
                sfx_dict[sfx.type] = sfx.clip;  
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }

    //UI에서 BGM과 SFX를 On/off 기능 구현. (OnValueChanged 로 호출할 함수)
    public void PlayBGM(BGM bgm_type)
    {
        //bgm이 존재하면 플레이 진행
        //C# 매개변수 한정자 out
        if (bgm_dict.TryGetValue(bgm_type, out var clip))
        {
            if (bgm.clip == clip) 
                return;

            bgm.clip = clip;
            bgm.loop = true;
            bgm.Play();
        }
    }

    public void PlaySFX(SFX sfx_type)
    {
        if (sfx_dict.TryGetValue(sfx_type, out var clip))
        {
            
            sfx.PlayOneShot(clip);
        }
    }

    //AudioMixer 볼륨 단위는 0db ~ -80db로 설정되어있음.
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(bgmParameter, Mathf.Log10(volume) * 20);
        //슬라이더 UI 최소값이 0.0001f 최대값 1f
    }   
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxParameter, Mathf.Log10(volume) * 20);
    }

    public void MuteBGM(bool mute)
    {
        audioMixer.SetFloat(bgmParameter, mute ? -80.0f : 0f); //삼항연산자. ? 뒤에 : 로 T,F 구별
    }

    public void MuteSFX(bool mute)
    {
        audioMixer.SetFloat(bgmParameter, mute ? -80.0f : 0f);
    }

    
}
