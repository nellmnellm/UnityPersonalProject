using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("버튼 및 UI 오브젝트 연결")]
    public GameObject titleGroup;            // Title (비활성화할 대상)
    public GameObject characterSelectUI;     // 캐릭터 선택창
    public GameObject infoPopup;             // 정보창 (Image + Text)

    public Button button1;                   // 캐릭터 선택 버튼
    public Button button2;                   // 정보창 토글 버튼
    public Button button3;                   // 종료 버튼

    public Button practiceToggleButton;
    public GameObject practiceButtonsGroup;

    public Button practiceStage1Button;
    public Button practiceStage2Button;
    public Button practiceStage3Button;

    void Start()
    {
        // 만일 게임하다가 돌아왔을때 playerManager 제거
       
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

        // 버튼 이벤트 할당
        button1.onClick.AddListener(OnClickShowCharacterSelect);
        button2.onClick.AddListener(OnClickToggleInfoPopup);
        button3.onClick.AddListener(OnClickQuitGame);

        practiceToggleButton.onClick.AddListener(TogglePracticeButtons);
        practiceStage1Button.onClick.AddListener(() => StartPracticeStage("Stage 1"));
        practiceStage2Button.onClick.AddListener(() => StartPracticeStage("Stage 2"));
        practiceStage3Button.onClick.AddListener(() => StartPracticeStage("Stage 3"));

        // 연습 버튼 초기 비활성화
        practiceButtonsGroup.SetActive(false);
        // 팝업 클릭 시 꺼지도록 리스너 추가
        if (infoPopup != null)
        {
            Button popupButton = infoPopup.GetComponent<Button>();
            if (popupButton != null)
            {
                popupButton.onClick.AddListener(HideInfoPopup);
            }
        }

        // 시작 시 팝업 및 캐릭터 선택창 비활성화
        characterSelectUI.SetActive(false);
        infoPopup.SetActive(false);
    }
    //연습모드 버튼 초기 비활성화
    public void TogglePracticeButtons()
    {
        bool isActive = practiceButtonsGroup.activeSelf;
        practiceButtonsGroup.SetActive(!isActive);
    }
    //연습모드 
    void StartPracticeStage(string stageName)
    {
        PlayerPrefs.SetInt("IS_PRACTICE", 1); // 연습모드 ON
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
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료
#else
        Application.Quit(); // 빌드 시 종료
#endif
    }
}