using System.Collections;
using TMPro;
using UnityEngine;

public class DialogControllerQuest : DialogController
{
    //������Ʈ
    //1. ����(Text)
    //2. ����(Text)
    public TMP_Text LabelTitle;
    public TMP_Text LabelQuestion;
    public TMP_Text LabelProcess;
    public GameObject QuestQuestion; //����õ�� ���� ������Ʈ ��ü
    public GameObject QuestProcess;  //���μ��� ���� ������Ʈ ��ü
    public bool questSuccess = false;

    public GameObject Button1; //����Ʈ ������ ��Ȱ��ȭ�� ����
    public GameObject Button2; // ����
    //������Ƽ
    //DialogDataConfirm
    DialogDataQuest Data { get; set; }


    //�������̵� 
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        //�Ŵ����� ���
        DialogManager.Instance.Regist(DialogType.Quest, this);
        QuestQuestion.SetActive(true);
        QuestProcess.SetActive(false);
        window.gameObject.SetActive(false);
        
    }


    public override void Build(DialogData data)
    {
        base.Build(data);
        this.DialogData = data;
        //������ ���� Ȯ��
        if (!(data is DialogDataQuest))
        {
            Debug.LogError("Invalid dialog Data!");
            return;
        }
        //�޼��� ���
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
        "����Ʈ Ŭ����!",
        "������ ������ϴ�!"
    );

        // Alert�� ���� �ڿ� Quest ���̾�α� Pop �ϵ��� ó��
        DialogManager.Instance.Push(alert);
    }

    public void OnYesButtonClick()
    {
        StartCoroutine(WaitForQuestSuccess());
    }
    public void OnNoButtonClick()
    {
        //�ݹ� ȣ��
        if (Data != null && Data.Callback != null)
        {
            Data.Callback(false);
        }
        //Pop
        DialogManager.Instance.Pop();
    }


}
