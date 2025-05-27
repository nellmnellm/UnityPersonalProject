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

    public Button practiceToggleButton;
    public GameObject practiceButtonsGroup;

    public Button practiceStage1Button;
    public Button practiceStage2Button;
    public Button practiceStage3Button;

    void Start()
    {
        // ���� �����ϴٰ� ���ƿ����� playerManager ����
       
        if (PlayerManager.Instance != null)
        {
            Destroy(PlayerManager.Instance.gameObject);
        }

        if (SoundManager.Instance != null)
        {
            Destroy(SoundManager.Instance.gameObject);
        }
        if (ScoreManager.Instance != null)
        {
            Destroy(ScoreManager.Instance.gameObject);
        }

        PlayerPrefs.SetInt("IS_PRACTICE", 0);
        PlayerPrefs.Save();

        // ��ư �̺�Ʈ �Ҵ�
        button1.onClick.AddListener(OnClickShowCharacterSelect);
        button2.onClick.AddListener(OnClickToggleInfoPopup);
        button3.onClick.AddListener(OnClickQuitGame);

        practiceToggleButton.onClick.AddListener(TogglePracticeButtons);
        practiceStage1Button.onClick.AddListener(() => StartPracticeStage("Stage 1"));
        practiceStage2Button.onClick.AddListener(() => StartPracticeStage("Stage 2"));
        practiceStage3Button.onClick.AddListener(() => StartPracticeStage("Stage 3"));

        // ���� ��ư �ʱ� ��Ȱ��ȭ
        practiceButtonsGroup.SetActive(false);
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
    //������� ��ư �ʱ� ��Ȱ��ȭ
    public void TogglePracticeButtons()
    {
        bool isActive = practiceButtonsGroup.activeSelf;
        practiceButtonsGroup.SetActive(!isActive);
    }
    //������� 
    void StartPracticeStage(string stageName)
    {
        PlayerPrefs.SetInt("IS_PRACTICE", 1); // ������� ON
        PlayerPrefs.Save();
        SceneManager.LoadScene(stageName);
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