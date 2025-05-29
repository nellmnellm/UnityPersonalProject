using UnityEngine;
using UnityEngine.Audio;

public static class SongLoader
{
    public static SongData SelectedSong;
}

public class GameManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        //song = SongLoader.SelectedSong;
        //audioSource.clip = song.audioClip;
        //titleImage.sprite = song.titleImage;
        //var difficulty = song.difficulty;
        //var songName = song.songName;
    }

    //public SongData SelectedSong;     //º±≈√«— ≥Î∑°.
}