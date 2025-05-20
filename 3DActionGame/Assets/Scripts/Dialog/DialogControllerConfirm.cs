using TMPro;
using UnityEngine;

public class DialogControllerConfirm : DialogController
{
    //������Ʈ
    //1. ����(Text)
    //2. ����(Text)
    public TMP_Text LabelTitle;
    public TMP_Text LabelContent;
    //������Ƽ
    //DialogDataConfirm
    DialogDataConfirm Data { get; set; }


    //�������̵� 
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        //�Ŵ����� ���
        DialogManager.Instance.Regist(DialogType.Confirm, this);
        window.gameObject.SetActive(false);
    }


    public override void Build(DialogData data)
    {
        base.Build(data);
        this.DialogData = data;
        //������ ���� Ȯ��
        if (!(data is DialogDataConfirm))
        {
            Debug.LogError("Invalid dialog Data!");
            return;
        }
        //�޼��� ���
        Data = data as DialogDataConfirm;
        LabelTitle.text = Data.Title;
        LabelContent.text = Data.Message;
    }

    public void OnYesButtonClick()
    {
        //�ݹ� ȣ��
        if (Data != null && Data.Callback != null)
        {
            Data.Callback(true);
        }
        //Pop
        DialogManager.Instance.Pop();
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
