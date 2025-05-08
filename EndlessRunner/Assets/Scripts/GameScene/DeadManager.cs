using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadManager : MonoBehaviour
{
    public TMP_Text current_score;
    public TMP_Text high_score;

   



    private void Awake()
    {
        gameObject.SetActive(false);
    }

    
    public void SetScoreText(float score)
    {
        gameObject.SetActive(true);
        current_score.text = ((int)(score)).ToString();

        //���࿡ ���޹��� ������ ���� ������Ʈ���� ���̽��ھ�� ũ�ٸ�
        if (score > PlayerPrefs.GetInt("HIGH_SCORE"))
        {
            //���Ӱ� �����մϴ�.
            PlayerPrefs.SetInt("HIGH_SCORE", (int)score);
        }

        high_score.text = PlayerPrefs.GetInt("HIGH_SCORE").ToString();
    }
   

    public void OnReplayButtonEnter()
    {
        //GetActiveScene()�� ���� Ȱ��ȭ�Ǿ��ִ� ���� �ǹ��մϴ�.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTitleButtonEnter()
    {
        SceneManager.LoadScene("TitleScene");
    }


}