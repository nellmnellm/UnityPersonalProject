using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public static StageController Instance;

    public int StagePoint = 0;
    public TMP_Text pointText;
    public Image fade_Image;
    public float duration = 5.0f;

    private void Awake()
    {
        Instance = this;   
    }

    private void Start()
    {

        //페이드 인
        if (fade_Image != null)
        {
            fade_Image.gameObject.SetActive(true);
            Color c = fade_Image.color;
            fade_Image.color = new Color(c.r, c.g, c.b, 1.0f);
            StartCoroutine(FadeIn());
        }

        //안내문 데이터 콜백
        DialogDataAlert alert = new DialogDataAlert("START", "10초마다 생성되는 슬라임들을 제거하세요.",
            () =>
            {
                Debug.Log("OK 버튼을 눌러주세요.");
            });

        DialogManager.Instance.Push(alert);
        StartSlimeQuest();
        
    }

    IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0, 1));
    }


    IEnumerator Fade(float start, float end)
    {
        float time = 0.0f;
        Color s = new Color(0, 0, 0, start);
        Color e = new Color(0, 0, 0, end);

        while (time < duration)
        {
            fade_Image.color = Color.Lerp(s, e, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fade_Image.color = e;
    }



    //점수가 증가하면, 텍스트 UI에 수치를 적용
    public void AddPoint(int Point)
    {
        StagePoint += Point;
        pointText.text = Point.ToString();


    }
    public void StartSlimeQuest()
    {
        DialogDataQuest quest = new DialogDataQuest(
            "Slime Kill",
            "Slime 1mari church",
            answer =>
            {
                if (answer)
                {
                    Debug.Log("퀘스트 수락됨");
                }
                else
                {
                    Debug.Log("퀘스트 거절됨");
                }
            }
        );

        DialogManager.Instance.Push(quest);
    }

    //씬에 대한 리로드
    public void FinishGame()
    {
        DialogDataConfirm confirm = new DialogDataConfirm("Restart?", "Please press OK if you want to restart the game"
            ,
            delegate (bool answer)
            {
                if (answer)
                {
                    Debug.Log("씬 리로드 시도됨");
                    StartCoroutine(FadeOut());
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else
                {
                    //이전 프로젝트의 내용을 활용해서 에디터 상에서도 종료되도록 수정해주세요.
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
         );

        //매니저에 등록
        DialogManager.Instance.Push(confirm);
    }
}