using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleButton : MonoBehaviour
{

    [SerializeField] private GameObject settingImage;
    [SerializeField] private GameObject exitingImage;
    
    public void OpenMusicScene()
    {
        SceneManager.LoadScene("SongSelectScene"); 
        //추후 노래 선곡씬으로 수정★
    }

    public void OpenSetting()
    {
        settingImage.SetActive(true);
    }

    public void OpenQuitAlert()
    {
        exitingImage.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // 에디터에서 플레이 중일 땐 강제 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
       // 실제 빌드된 게임에서는 이걸로 종료
       Application.Quit();
#endif
    }


}
