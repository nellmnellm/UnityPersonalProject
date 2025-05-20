using System.Collections;
using TMPro;
using UnityEngine;

public class DialogControllerQuest : DialogController
{
    //컴포넌트
    //1. 제목(Text)
    //2. 내용(Text)
    public TMP_Text LabelTitle;
    public TMP_Text LabelQuestion;
    public TMP_Text LabelProcess;
    public GameObject QuestQuestion; //퀘스천을 담은 오브젝트 자체
    public GameObject QuestProcess;  //프로세스 담은 오브젝트 자체
    public bool questSuccess = false;

    public GameObject Button1; //퀘스트 수락시 비활성화를 위해
    public GameObject Button2; // 같은
    //프로퍼티
    //DialogDataConfirm
    DialogDataQuest Data { get; set; }


    //오버라이드 
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        //매니저에 등록
        DialogManager.Instance.Regist(DialogType.Quest, this);
        QuestQuestion.SetActive(true);
        QuestProcess.SetActive(false);
        window.gameObject.SetActive(false);
        
    }


    public override void Build(DialogData data)
    {
        base.Build(data);
        this.DialogData = data;
        //데이터 여부 확인
        if (!(data is DialogDataQuest))
        {
            Debug.LogError("Invalid dialog Data!");
            return;
        }
        //메세지 등록
        Data = data as DialogDataQuest;
        LabelTitle.text = Data.Title;
        LabelQuestion.text = Data.Message;
    }
    private IEnumerator WaitForQuestSuccess()
    {
        QuestQuestion.SetActive(false);
        QuestProcess.SetActive(true);
        Button1.SetActive(false);
        Button2.SetActive(false);
        if (Data != null && Data.Callback != null)
        {
            Data.Callback(true);
        }

        yield return new WaitUntil(() => questSuccess);

        QuestQuestion.SetActive(true);
        QuestProcess.SetActive(false);
        Button1.SetActive(true);
        Button2.SetActive(true);

        DialogManager.Instance.Pop();

        DialogDataAlert alert = new DialogDataAlert(
        "퀘스트 클리어!",
        "보상을 얻었습니다!"
    );

        // Alert가 끝난 뒤에 Quest 다이얼로그 Pop 하도록 처리
        DialogManager.Instance.Push(alert);
    }

    public void OnYesButtonClick()
    {
        StartCoroutine(WaitForQuestSuccess());
    }
    public void OnNoButtonClick()
    {
        //콜백 호출
        if (Data != null && Data.Callback != null)
        {
            Data.Callback(false);
        }
        //Pop
        DialogManager.Instance.Pop();
    }


}
