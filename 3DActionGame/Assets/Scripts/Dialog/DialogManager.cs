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
    Dictionary<DialogType, DialogController> _dialogdict;   //��Ʈ�ѷ� ���
    DialogController _currentDialogController; //������ ���̾�α� ��Ʈ�ѷ�

    
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
        // �̹� ��⿭�� ���� �����Ͱ� �ִٸ� �߰����� ���� (�ɼ�)
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
        //���� �����ִ� ��ȭâ�� �ݰ�, �����ִ� ���� ��ȭâ�� �����ִ� ����
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
        //����Ʈ�� ù��° �� ����

        //�ش� ���� ��ųʸ��� Ű�ν� ������, ��Ʈ�ѷ��� ��ȸ�մϴ�.
        DialogController controller = _dialogdict[next.Type].GetComponent<DialogController>();

        _currentDialogController = controller;

        _currentDialogController.Build(next);

        _currentDialogController.Show(() => { }); //��ȭâ�� ���� ���� ��ó���� ���� �ʴ´�.

        _dialogList.RemoveAt(0); //����Ʈ���� ù��° �� ����
    }

    public bool IsShowing()
    {
        return _currentDialogController != null;
    }
}