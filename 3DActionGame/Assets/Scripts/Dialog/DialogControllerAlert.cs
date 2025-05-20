using TMPro;
using UnityEngine;

public class DialogControllerAlert : DialogController
{
    //������Ʈ ����
    //1. Ÿ��Ʋ Text
    //2. ���� Text
    public TMP_Text LabelTitle;
    public TMP_Text LabelContent;

    //������Ƽ
    //�˸� â�� ������
    DialogDataAlert Data { get; set; }

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        //�Ŵ����� ���� ����(Ÿ�� : Alert, ��Ʈ�ѷ� : �ڱ� �ڽ�(Controller Alert))
        DialogManager.Instance.Regist(DialogType.Alert, this);
        window.gameObject.SetActive(false);
    }
    public override void Build(DialogData data)
    {
        base.Build(data);
        this.DialogData = data;
        //�����Ͱ� DialogDataAlert ���°� �ƴ� ���¿��� ���带 ������ ���
        if (!(data is DialogDataAlert))
        {
            Debug.LogError("Invalid dialog Data!");
            return;
        }

        Data = data as DialogDataAlert;
        LabelTitle.text = Data.Title;
        LabelContent.text = Data.Message;
    }
    public void OnOKButtonClick()
    {
        //�����͵� �ְ�, �ݹ鵵 ��û�ߴٸ�
        if (Data != null && Data.Callback != null)
        {
            Data.Callback();
        }
        //�ݹ� ����, �Ŵ������� ����
        DialogManager.Instance.Pop();
    }
}
