using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelectManager : MonoBehaviour
{
    //�����ư�� inspector���� OnSongSelected ����
    
    public GameObject editorButtons;  //������ ��ư�� ��� �׸�. ��ư ���ڴ� OnSongSelectedEdit ����

    public GameObject chartButtonPrefab;          // Chart ��Ͽ� ��ư ������
    public Transform chartListContent;            // ��ũ�Ѻ� - ������ ������Ʈ�� �ֱ�.

    public GameObject[] songButtons;  // 3�� ��ư �ֱ�. 3���߿� �ϳ��� ��Ȱ��ȭ�ϱ� ����
    public SongData[] allSongs;      // 3�� �� ���� SO ���� ����
    private int currentSelectedIndex = -1; //�⺻�� 
    //songName / composer / difficulty �� ������ ��Ÿ���ִ� UI
    public TMP_Text songNameText;
    public TMP_Text songComposerText;
    public TMP_Text songDifficultyText;
    
    public Image fadePanel; // ���� �̹���
    private bool isExiting = false;
    private float holdTime = 0f;
    private const float requiredHoldDuration = 3f;

    public SceneEscSound escSound;//������ esc �������� ����

    public SongPreviewPlayer previewPlayer;

    IEnumerator Start()
    {
        if (editorButtons != null)
            editorButtons.SetActive(false); // �⺻ ��Ȱ��ȭ
        yield return null;  
        OnSongButtonClicked(0);//�⺻���� 0�� ����
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
                PopulateChartListForEditorBySelectedSong();
            }
        }
        else
        {
            holdTime = 0f; // ������ ���� ������ �ʱ�ȭ
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isExiting)
        {
            StartCoroutine(FadeAndExit());
        }

        if (Input.GetKeyDown(KeyCode.Return) && currentSelectedIndex >= 0 && !isExiting)
        {
            OnSongSelected(allSongs[currentSelectedIndex]);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSongByOffset(2);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSongByOffset(1);
        }

        
    }

    private void PopulateChartListForEditorBySelectedSong()
    {
        if (currentSelectedIndex < 0 || currentSelectedIndex >= allSongs.Length)
        {
            Debug.LogWarning("���õ� �� �ε����� ��ȿ���� �ʽ��ϴ�.");
            return;
        }

        SongData song = allSongs[currentSelectedIndex];
        string baseName = Path.GetFileNameWithoutExtension(song.videoFileName); // ��: CatRave

        // ���� ��ư ����
        foreach (Transform child in chartListContent)
            Destroy(child.gameObject);

        string chartDir = Path.Combine(Application.streamingAssetsPath, "Charts");
        Directory.CreateDirectory(chartDir);

        string[] allJsonFiles = Directory.GetFiles(chartDir, "*.json");

        foreach (string path in allJsonFiles)
        {
            string fileName = Path.GetFileName(path);

            // baseName ���� �����ϴ� json�� ǥ��
            if (!fileName.StartsWith(baseName)) continue;

            GameObject btn = Instantiate(chartButtonPrefab, chartListContent);
            btn.GetComponentInChildren<TMP_Text>().text = fileName;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                SongLoader.SelectedSong = song; 
                SongLoader.SelectedChartFileName = fileName;
                SceneManager.LoadScene("MusicScene");
            });
        }
    }

    private void ChangeSongByOffset(int offset)
    {
        currentSelectedIndex = (currentSelectedIndex + offset) % songButtons.Length;

        OnSongButtonClicked(currentSelectedIndex);

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
        previewPlayer.StopPreview();

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

        previewPlayer.PlayPreview(selected.previewClip);
    }

    private IEnumerator FadeAndExit()
    {
        escSound.PlayEscSound();
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