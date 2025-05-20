using System;
using UnityEngine;

public class DialogDataQuest : DialogData
{
    //������Ƽ
    //1. ����
    //2. ����
    //3. �׼�
    public string Title { get; private set; }

    public string Message { get; private set; }


    public Action<bool> Callback { get; private set; }

    public DialogDataQuest(string title, string message, Action<bool> callback = null) : base(DialogType.Quest)
    {
        Title = title;
        Message = message;
        this.Callback = callback;
    }

    //������ 



}