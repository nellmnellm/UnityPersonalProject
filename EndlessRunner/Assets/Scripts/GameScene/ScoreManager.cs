using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public PlayerController playerController;
    public DeadManager deadManager;   //���� �Ŵ���

    //����
    //1. �ΰ��� ������ �����ϰ� ����ҰŸ� �ʵ�� �����.
    //2. ������ ���� �� �ڿ��� ������ �����Ǿ� �ϴ� ��Ȳ�� �����
    //   ������ ���� ������� ����մϴ�.
    //   2-1. PlayerPrefs : ����Ƽ�� �����͸� ������Ʈ���� ������ �� ����մϴ�.
    //                      ����,�Ǽ�,���� ������ ������ ������ ���� ����
    //                      ���� ���ž��ϸ� ������ �����ص� �����ִ� ��찡 �����.

    //   2-2. Json        : JavaScript Object Notation : ������ ���ۿ� ����
    //                      ��ü, �迭, ���ڿ� , Boolean, Null, ���� ���� ������ ���� ����
    //                      �ַ� ���ӿ��� ��ųʸ��� ����Ʈ ���� ���ؼ� ������ ���̺�, �κ��丮
    //                      ���� ���̺� ���� �����ҷ��� �� �� ���ϰ� ����ϴ� �뵵

    //   2-3. Firebase    : �����ͺ��̽����� ������ ���� �����͸� �����մϴ�.(��Ƽ ����)

    //   2-4. ScriptableObject : ����Ƽ ���ο� Asset�� ���·ν� �����͸� �����ؼ� ����մϴ�.
    //                           �ΰ��� �����͸� ������ �� ���� ���� ���մϴ�.

    //   2-5. CSV         : ���� ���Ͽ� �ʿ��� �����͵��� �����صΰ�, C# ��ũ��Ʈ�� ���� �ش� ���� ���ͼ�
    //                      �����մϴ�. �ַ� �� ���� , ��ũ��Ʈ ��ȭ ȣ��, �⺻���� ������
    private float score = 0.0f;

    //�ؽ�Ʈ UI
    public TMP_Text scoreText;
    public TMP_Text HighScore;
    public TMP_Text START;
    //���� ���� üũ
    [SerializeField] private bool DeadCheck = false;

    [SerializeField] private bool GameStart = false;

    IEnumerator GameStartWait()
    { 
        yield return new WaitForSeconds(3f);
        GameStart = true;
        START.text = "START!!";
        yield return new WaitForSeconds(1f);
        START.text = "";
    }
    
    private void Start()
    {
        StartCoroutine(GameStartWait());
        //HIGH_SCORE Ű�� ���ٸ�, Ű�� ���� ����
        if (PlayerPrefs.HasKey("HIGH_SCORE") == false)
        {
            Debug.Log("HIGH_SCORE Ű�� ���ŵǾ����ϴ�.");
            PlayerPrefs.SetInt("HIGH_SCORE", 0);
        }
        else
        {
            Debug.Log("Ű�� �����մϴ�!");
        }

        //�ְ� ���� ����
        HighScore.text = $"High\n{PlayerPrefs.GetInt("HIGH_SCORE")}";

        
    }
    //String Format $
    // $"{����}"�� ���� ��� �ش� ������ ���ڿ��� �Ѿ�� �˴ϴ�. 
    // {����:N0} : ���� ��� �� ,�� ǥ���� �� �ֽ��ϴ�. 1000 -> 1,000 
    // #,##0 : #�� 0�� �տ� �Ⱥ���. ���ڰ� ������ ǥ��, ������ ǥ�� ����
    //         0�� �տ� ����. ���ڰ� ������ ����ǥ��, ������ 0 ǥ��
   
    void Update()
    {
        if (!GameStart)
            return;
        //���� ������ ��� ���ھ ���̻� ������ �ʽ��ϴ�.
        //�÷��̾�� UI���� ���� �������� ó���ǰ� ���� �����
        //static ������ �����ͷ� ó���ϴ� �͵� �������ϴ�.
        if (DeadCheck)
        {
            return;
        }

        score += Time.deltaTime * 30;


        scoreText.text = $"Score\n{((int)score).ToString()}";

        //score�� ���̽��ھ� ���� �Ѿ��� ����� �ؽ�Ʈ ����
        if (score > PlayerPrefs.GetInt("HIGH_SCORE"))
        {
            //�ش� �ڵ带 ����ϸ� ��� ������ ���� �����Ǳ� ������ ����� �����ְ�, Dead���� ���� 1������ ó��
            //PlayerPrefs.SetInt("HIGH_SCORE", (int)score);
            //HighScore.text = $"High Score : {PlayerPrefs.GetInt("HIGH_SCORE")}"; 
            HighScore.text = $"High\n{((int)score).ToString()}";
            HighScore.color = Color.red;
        }

    }
   

    public void OnDead()
    {
        DeadCheck = true;
        //�Ŵ����� ���� ����
        deadManager.SetScoreText(score);
    }
}