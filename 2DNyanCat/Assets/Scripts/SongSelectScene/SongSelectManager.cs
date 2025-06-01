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
}