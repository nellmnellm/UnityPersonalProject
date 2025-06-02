using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour
{
    //�����ư�� inspector���� OnSongSelected ����
    
    public GameObject editorButtons;  //������ ��ư�� ��� �׸�. ��ư ���ڴ� OnSongSelectedEdit ����

    public GameObject[] songButtons;  // 3�� ��ư �ֱ�. 3���߿� �ϳ��� ��Ȱ��ȭ�ϱ� ����
    public SongData[] allSongs;      // 3�� �� ���� SO ���� ����
    private int currentSelectedIndex = -1; //�⺻�� 
    //songName / composer / difficulty �� ������ ��Ÿ���ִ� UI
    public TMP_Text songNameText;
    public TMP_Text songComposerText;
    public TMP_Text songDifficultyText;


    private float holdTime = 0f;
    private const float requiredHoldDuration = 3f;

    void Start()
    {
        if (editorButtons != null)
            editorButtons.SetActive(false); // �⺻ ��Ȱ��ȭ
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= requiredHoldDuration && editorButtons != null && !editorButtons.activeSelf)
            {
                editorButtons.SetActive(true);
                Debug.Log("������ ��ư Ȱ��ȭ��!");
            }
        }
        else
        {
            holdTime = 0f; // ������ ���� ������ �ʱ�ȭ
        }
    }

    public void OnSongSelected(SongData selectedSong)
    {
        SongLoader.SelectedSong = selectedSong;  //songLoader�� GameManager�ʿ� ����
        SceneManager.LoadScene("MusicScene");
    }

    public void OnSongSelectedEdit(SongData selectedSong)
    {
        SongLoader.SelectedSong = selectedSong;  //songLoader�� GameManager�ʿ� ����
        SceneManager.LoadScene("EditorScene");
    }
    public void OnSongButtonClicked(int index)
    {
        // �� ���� UI ������Ʈ
        SongData selected = allSongs[index];
        songNameText.text = selected.songName;
        songComposerText.text = selected.composer;
        songDifficultyText.text = selected.difficulty;

        // ���õ� �� ����
        SongLoader.SelectedSong = selected;
        currentSelectedIndex = index;

        // ��ư ���� ����
        for (int i = 0; i < songButtons.Length; i++)
        {
            songButtons[i].SetActive(i != index); // �ڱ� �ڽ��� ��Ȱ��ȭ, �������� Ȱ��ȭ
        }

    }
}