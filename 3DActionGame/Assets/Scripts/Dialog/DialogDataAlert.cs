using System;
using UnityEngine;

public class DialogDataAlert : DialogData
{
    //������Ƽ ����
    //1. ����
    //2. �޼���
    //3. ��ư ���� ���� �ݹ� �Լ�

    //�ش� �������� ������Ƽ���� Ŭ���� ���ο����� ������ �����մϴ�.

    public string Title { get; private set; }

    public string Message { get; private set; }

    public Action Callback { get; private set; }

    //Ŭ���� ���� �� ���� ����)
    public DialogDataAlert(string title, string message, Action callback = null) : base(DialogType.Alert)
    {
        Title = title;
        Message = message;
        Callback = callback;
    }
}
