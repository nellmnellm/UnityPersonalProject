using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class TitleManager : MonoBehaviour
{
    public GameObject ControlKeyMenu;
    public GameObject ExitMenu;
    public void OnStartButtonEnter()
    {
        // "GameScene"�� ��ȯ�� �� �̸�
        SceneManager.LoadScene("GameScene");
        //SceneManager.LoadScene(0);
    }

    public void OnControlKeyButtonEnter()
    {
        ControlKeyMenu.SetActive(true);  
        
    }

    public void OnExitButtonEnter()
    {
        ExitMenu.SetActive(true);
    }


    public void Exit()
    {
#if UNITY_EDITOR //����Ƽ �����Ϳ����� �۾�
    UnityEditor.EditorApplication.isPlaying = false;
#else        //������ ���α׷� ����
        Application.Quit();
#endif
    }

   
}

