using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.Audio;

//���� ������ ���� Enum ����
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
    [Header("����� �ͼ�")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM"; //����� �ͼ��� �̸� => �������� ����ȭ
    public string sfxParameter = "SFX";

    [Header("����� �ҽ�")]
    public AudioSource bgm;
    public AudioSource sfx;

    [Header("����� Ŭ��")]
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
            //��ųʸ� ����
            bgm_dict = new Dictionary<BGM, AudioClip>();
            sfx_dict = new Dictionary<SFX, AudioClip>();
            //����Ʈ�� �ִ� �����͵� ��ųʸ��� ���.
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

    //UI���� BGM�� SFX�� On/off ��� ����. (OnValueChanged �� ȣ���� �Լ�)
    public void PlayBGM(BGM bgm_type)
    {
        //bgm�� �����ϸ� �÷��� ����
        //C# �Ű����� ������ out
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

    //AudioMixer ���� ������ 0db ~ -80db�� �����Ǿ�����.
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(bgmParameter, Mathf.Log10(volume) * 20);
        //�����̴� UI �ּҰ��� 0.0001f �ִ밪 1f
    }   
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxParameter, Mathf.Log10(volume) * 20);
    }

    public void MuteBGM(bool mute)
    {
        audioMixer.SetFloat(bgmParameter, mute ? -80.0f : 0f); //���׿�����. ? �ڿ� : �� T,F ����
    }

    public void MuteSFX(bool mute)
    {
        audioMixer.SetFloat(bgmParameter, mute ? -80.0f : 0f);
    }

    
}
