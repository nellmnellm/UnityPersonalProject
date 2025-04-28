using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//대리자 기능 중 하나.
//Action이나 Func는 C# 형식의 대리자임.
public class UnityEventSample2 : MonoBehaviour
{
    public UnityEvent OnQuest;

    public Text clear_count_text;
    public Text current_count_text;
    public Text message;

    public int clear_count = 10;
    public int current_count = 0;
    void countPlus() => current_count++; // 개수 증가 (1개)

    void countText()
    {
        current_count_text.text = $"{current_count}개";
        message.text = $"[퀘스트 진행중..] {current_count} / {clear_count}개";
    }

    void QuestClear()
    {
        if (current_count == clear_count)
        {
            clear_count_text.text = "끝";
            current_count_text.text = "끝";
            message.text = "퀘스트 완료";
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
    {//인스펙터로는 addListener 확인이 불가능ㅎㅁ
        
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
            Debug.Log("등록된 비영구리스너가 없습니다");

            //비영구 => 스크립트를 통해서 추가한 리스너. (AddListener)
            //영구 : 인스펙터를 통해 추가함. 인스펙터에서 직접 지워야함.
        }
    
    }
}
