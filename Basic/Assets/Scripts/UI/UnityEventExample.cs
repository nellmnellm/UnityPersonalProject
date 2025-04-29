using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//�븮�� ��� �� �ϳ�.
//Action�̳� Func�� C# ������ �븮����.
public class UnityEventSample2 : MonoBehaviour
{
    public UnityEvent OnQuest;

    public Text clear_count_text;
    public Text current_count_text;
    public Text message;

    public int clear_count = 10;
    public int current_count = 0;
    void countPlus() => current_count++; // ���� ���� (1��)

    void countText()
    {
        current_count_text.text = $"{current_count}��";
        message.text = $"[����Ʈ ������..] {current_count} / {clear_count}��";
    }

    void QuestClear()
    {
        if (current_count == clear_count)
        {
            clear_count_text.text = "��";
            current_count_text.text = "��";
            message.text = "����Ʈ �Ϸ�";
            clear_count_text.color = Color.gray;
            clear_count_text.color = Color.gray;
            message.color = Color.cyan;
            OnQuest.RemoveAllListeners();
            OnQuest.IsUnityNull();
        }

        else
        {
            countText();
        }

    }
        

    private void Start()
    {//�ν����ͷδ� addListener Ȯ���� �Ұ��ɤ���
        
        OnQuest.AddListener(countPlus);
        OnQuest.AddListener(QuestClear);
    }

    private void Update()
    {
        if (OnQuest != null)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnQuest.Invoke();
            }
        }
        else
        {
            Debug.Log("��ϵ� �񿵱������ʰ� �����ϴ�");

            //�񿵱� => ��ũ��Ʈ�� ���ؼ� �߰��� ������. (AddListener)
            //���� : �ν����͸� ���� �߰���. �ν����Ϳ��� ���� ��������.
        }
    
    }
}
