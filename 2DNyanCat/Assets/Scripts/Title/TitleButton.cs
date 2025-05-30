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
        //���� �뷡 ��������� ������
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
        // �����Ϳ��� �÷��� ���� �� ���� ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
       // ���� ����� ���ӿ����� �̰ɷ� ����
       Application.Quit();
#endif
    }


}
