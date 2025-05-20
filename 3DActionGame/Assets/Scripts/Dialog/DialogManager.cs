using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public enum DialogType
{
    Alert = 1, 
    Confirm = 2, 
    Quest = 0,
}
public sealed class DialogManager : MonoBehaviour
{
    List<DialogData> _dialogList;                           
    Dictionary<DialogType, DialogController> _dialogdict;   //컨트롤러 사용
    DialogController _currentDialogController; //현재의 다이얼로그 컨트롤러

    
    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public DialogManager()
    {
        _dialogList = new List<DialogData>();
        _dialogdict = new Dictionary<DialogType, DialogController>();
    }

    public void Regist(DialogType type, DialogController controller)
    {
        _dialogdict[type] = controller;
    }

    public void Push(DialogData data)
    {
        // 이미 대기열에 같은 데이터가 있다면 추가하지 않음 (옵션)
        if (_dialogList.Contains(data))
        {
            Debug.LogWarning("Duplicate dialog data pushed.");
            return;
        }

        if (_currentDialogController == null)
        {
            _dialogList.Add(data);
            SortDialogList();
            ShowNext();
        }
        else
        {
            DialogType currentType = _currentDialogController.DialogData.Type;

            if ((int)data.Type > (int)currentType)
            {
                DialogData currentData = _currentDialogController.DialogData;

                _dialogList.Insert(0, data);
                _dialogList.Add(currentData);

                _currentDialogController.Close(() =>
                {
                    _currentDialogController = null;
                    SortDialogList();
                    ShowNext();
                });
            }
            else
            {
                _dialogList.Add(data);
                SortDialogList();
            }
        }
    }

    private void SortDialogList()
    {
        _dialogList = _dialogList
            .OrderByDescending(d => (int)d.Type)
            .ToList();
    }
    /*public void Push(DialogData data)
    {
        _dialogList.Add(data);

        *//*  if (_currentDialogController == null)
          {
              ShowNext();
          }*//*
        _dialogList = _dialogList
         .OrderByDescending(d => (int)d.Type)
         .ToList();

        if (_currentDialogController == null)
        {
            ShowNext();
        }
    }*/
    public void Pop()
    {
        //현재 열려있는 대화창을 닫고, 남아있는 다음 대화창을 보여주는 구조
        if (_currentDialogController != null)
        {
            _currentDialogController.Close(
                        delegate
                        {
                            _currentDialogController = null;

                            if (_dialogList.Count > 0)
                            {
                                ShowNext();
                            }
                        });
        }
    }


    private void ShowNext()
    {
        DialogData next = _dialogList[0];
        //리스트의 첫번째 값 지정

        //해당 값을 딕셔너리에 키로써 접근해, 컨트롤러를 조회합니다.
        DialogController controller = _dialogdict[next.Type].GetComponent<DialogController>();

        _currentDialogController = controller;

        _currentDialogController.Build(next);

        _currentDialogController.Show(() => { }); //대화창을 띄우되 따로 후처리를 하지 않는다.

        _dialogList.RemoveAt(0); //리스트에서 첫번째 값 제거
    }

    public bool IsShowing()
    {
        return _currentDialogController != null;
    }
}