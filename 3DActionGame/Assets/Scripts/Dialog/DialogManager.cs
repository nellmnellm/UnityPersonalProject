using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
public enum DialogType
{
    Alert, Confirm
}
public sealed class DialogManager : MonoBehaviour
{
    List<DialogData> _dialogList;                           
    Dictionary<DialogType, DialogController> _dialogdict;   //��Ʈ�ѷ� ���
    DialogController _currentDialogController; //������ ���̾�α� ��Ʈ�ѷ�

    private static DialogManager instance = new DialogManager();

  
    public static DialogManager Instance
    {
        get { return instance; }
    }

    public DialogManager()
    {
        _dialogList = new List<DialogData>();
        _dialogdict = new Dictionary<DialogType, DialogController>();
    }

    public void Regist(DialogData data)
    {
        _dialogList.Add(data);
        if (_currentDialogController == null)
            ShowNext();
    }
    



}
