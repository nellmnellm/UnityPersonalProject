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

        //���̵� ��
        if (fade_Image != null)
        {
            fade_Image.gameObject.SetActive(true);
            Color c = fade_Image.color;
            fade_Image.color = new Color(c.r, c.g, c.b, 1.0f);
            StartCoroutine(FadeIn());
        }

        //�ȳ��� ������ �ݹ�
        DialogDataAlert alert = new DialogDataAlert("START", "10�ʸ��� �����Ǵ� �����ӵ��� �����ϼ���.",
            () =>
            {
                Debug.Log("OK ��ư�� �����ּ���.");
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



    //������ �����ϸ�, �ؽ�Ʈ UI�� ��ġ�� ����
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
                    Debug.Log("����Ʈ ������");
                }
                else
                {
                    Debug.Log("����Ʈ ������");
                }
            }
        );

        DialogManager.Instance.Push(quest);
    }

    //���� ���� ���ε�
    public void FinishGame()
    {
        DialogDataConfirm confirm = new DialogDataConfirm("Restart?", "Please press OK if you want to restart the game"
            ,
            delegate (bool answer)
            {
                if (answer)
                {
                    Debug.Log("�� ���ε� �õ���");
                    StartCoroutine(FadeOut());
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else
                {
                    //���� ������Ʈ�� ������ Ȱ���ؼ� ������ �󿡼��� ����ǵ��� �������ּ���.
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
         );

        //�Ŵ����� ���
        DialogManager.Instance.Push(confirm);
    }
}