using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelectManager : MonoBehaviour
{
    //선곡버튼은 inspector에서 OnSongSelected 연결
    
    public GameObject editorButtons;  //에디터 버튼을 담는 그릇. 버튼 각자는 OnSongSelectedEdit 연결

    public GameObject[] songButtons;  // 3개 버튼 넣기. 3개중에 하나만 비활성화하기 위해
    public SongData[] allSongs;      // 3개 곡 정보 SO 파일 연결
    private int currentSelectedIndex = -1; //기본값 
    //songName / composer / difficulty 곡 정보를 나타내주는 UI
    public TMP_Text songNameText;
    public TMP_Text songComposerText;
    public TMP_Text songDifficultyText;
    
    public Image fadePanel; // 검정 이미지
    private bool isExiting = false;
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

        if (Input.GetKeyDown(KeyCode.Escape) && !isExiting)
        {
            StartCoroutine(FadeAndExit());
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
    public void OnSongButtonClicked(int index)
    {
        // 곡 정보 UI 업데이트
        SongData selected = allSongs[index];
        songNameText.text = selected.songName;
        songComposerText.text = selected.composer;
        songDifficultyText.text = selected.difficulty;

        // 선택된 곡 저장
        SongLoader.SelectedSong = selected;
        currentSelectedIndex = index;

        // 버튼 상태 갱신
        for (int i = 0; i < songButtons.Length; i++)
        {
            songButtons[i].SetActive(i != index); // 자기 자신은 비활성화, 나머지는 활성화
        }

    }

    private IEnumerator FadeAndExit()
    {
        isExiting = true;

        float duration = 0.5f;
        float elapsed = 0f;

        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = Color.Lerp(startColor, endColor, elapsed / duration);
                yield return null;
            }

            fadePanel.color = endColor;
        }

        SceneManager.LoadScene("Title");
    }
}