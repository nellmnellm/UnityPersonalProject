using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("��ư �� UI ������Ʈ ����")]
    public GameObject titleGroup;            // Title (��Ȱ��ȭ�� ���)
    public GameObject characterSelectUI;     // ĳ���� ����â
    public GameObject infoPopup;             // ����â (Image + Text)

    public Button button1;                   // ĳ���� ���� ��ư
    public Button button2;                   // ����â ��� ��ư
    public Button button3;                   // ���� ��ư

    void Start()
    {
        // ��ư �̺�Ʈ �Ҵ�
        button1.onClick.AddListener(OnClickShowCharacterSelect);
        button2.onClick.AddListener(OnClickToggleInfoPopup);
        button3.onClick.AddListener(OnClickQuitGame);

        // �˾� Ŭ�� �� �������� ������ �߰�
        if (infoPopup != null)
        {
            Button popupButton = infoPopup.GetComponent<Button>();
            if (popupButton != null)
            {
                popupButton.onClick.AddListener(HideInfoPopup);
            }
        }

        // ���� �� �˾� �� ĳ���� ����â ��Ȱ��ȭ
        characterSelectUI.SetActive(false);
        infoPopup.SetActive(false);
    }

    void OnClickShowCharacterSelect()
    {
        titleGroup.SetActive(false);
        characterSelectUI.SetActive(true);
    }

    void OnClickToggleInfoPopup()
    {
        bool isActive = infoPopup.activeSelf;
        infoPopup.SetActive(!isActive);
    }

    void HideInfoPopup()
    {
        infoPopup.SetActive(false);
    }

    void OnClickQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �����Ϳ��� ����
#else
        Application.Quit(); // ���� �� ����
#endif
    }
}