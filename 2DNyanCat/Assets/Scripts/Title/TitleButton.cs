using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleButton : MonoBehaviour
{
    [SerializeField] private GameObject settingImage;
    [SerializeField] private GameObject exitingImage;   
    /// <summary>
    /// 첫번째 버튼과 맵핑됨
    /// </summary>
    public void OpenMusicScene()
    {
        SceneManager.LoadScene("SongSelectScene"); 
    }
    /// <summary>
    /// 두번째 버튼과 맵핑됨.
    /// </summary>
    public void OpenSetting()
    {
        settingImage.SetActive(true);
    }
    /// <summary>
    /// 종료 창을 띄움
    /// </summary>
    public void OpenQuitAlert()
    {
        exitingImage.SetActive(true);
    }
    /// <summary>
    /// 종료 버튼과 맵핑됨
    /// </summary>
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
