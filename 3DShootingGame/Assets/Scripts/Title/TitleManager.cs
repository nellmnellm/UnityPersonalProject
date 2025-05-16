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

    void Start()
    {
        // 버튼 이벤트 할당
        button1.onClick.AddListener(OnClickShowCharacterSelect);
        button2.onClick.AddListener(OnClickToggleInfoPopup);
        button3.onClick.AddListener(OnClickQuitGame);

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