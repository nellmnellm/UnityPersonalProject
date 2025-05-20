using System;
using UnityEngine;

public class DialogDataConfirm : DialogData
{
    //������Ƽ
    //1. ����
    //2. ����
    //3. �׼�
    public string Title { get; private set; }

    public string Message { get; private set; }


    public Action<bool> Callback { get; private set; }

    public DialogDataConfirm(string title, string message, Action<bool> callback = null) : base(DialogType.Confirm)
    {
        Title = title;
        Message = message;
        this.Callback = callback;
    }

    //������ 



}