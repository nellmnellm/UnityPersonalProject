using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour
{
    public GameObject editorButtons;

    private float holdTime = 0f;
    private const float requiredHoldDuration = 3f;

    void Start()
    {
        if (editorButtons != null)
            editorButtons.SetActive(false); // 기본 비활성화
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= requiredHoldDuration && editorButtons != null && !editorButtons.activeSelf)
            {
                editorButtons.SetActive(true);
                Debug.Log("에디터 버튼 활성화됨!");
            }
        }
        else
        {
            holdTime = 0f; // 누르고 있지 않으면 초기화
        }
    }

    public void OnSongSelected(SongData selectedSong)
    {
        SongLoader.SelectedSong = selectedSong;  //songLoader는 GameManager쪽에 있음
        SceneManager.LoadScene("MusicScene");
    }

    public void OnSongSelectedEdit(SongData selectedSong)
    {
        SongLoader.SelectedSong = selectedSong;  //songLoader는 GameManager쪽에 있음
        SceneManager.LoadScene("EditorScene");
    }
}