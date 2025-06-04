using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleButton : MonoBehaviour
{
    [SerializeField] private GameObject settingImage;
    [SerializeField] private GameObject exitingImage;   
    /// <summary>
    /// ù��° ��ư�� ���ε�
    /// </summary>
    public void OpenMusicScene()
    {
        SceneManager.LoadScene("SongSelectScene"); 
    }
    /// <summary>
    /// �ι�° ��ư�� ���ε�.
    /// </summary>
    public void OpenSetting()
    {
        settingImage.SetActive(true);
    }
    /// <summary>
    /// ���� â�� ���
    /// </summary>
    public void OpenQuitAlert()
    {
        exitingImage.SetActive(true);
    }
    /// <summary>
    /// ���� ��ư�� ���ε�
    /// </summary>
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
