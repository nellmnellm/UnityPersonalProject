using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour
{
    public void OnSongSelected(SongData selectedSong)
    {
        SongLoader.SelectedSong = selectedSong;  //songLoader는 GameManager쪽에 있음
        SceneManager.LoadScene("MusicScene");
    }
}