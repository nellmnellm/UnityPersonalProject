using System;
using UnityEngine;

public class DialogDataConfirm : DialogData
{
    //프로퍼티
    //1. 제목
    //2. 내용
    //3. 액션
    public string Title { get; private set; }

    public string Message { get; private set; }


    public Action<bool> Callback { get; private set; }

    public DialogDataConfirm(string title, string message, Action<bool> callback = null) : base(DialogType.Confirm)
    {
        Title = title;
        Message = message;
        this.Callback = callback;
    }

    //생성자 



}