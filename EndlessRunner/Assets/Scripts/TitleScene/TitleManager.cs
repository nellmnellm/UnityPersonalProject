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
        // "GameScene"은 전환할 씬 이름
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
#if UNITY_EDITOR //유니티 에디터에서의 작업
    UnityEditor.EditorApplication.isPlaying = false;
#else        //누르면 프로그램 종료
        Application.Quit();
#endif
    }

   
}

