using UnityEngine;
using UnityEngine.Events;

//대리자 기능 중 하나.
//Action이나 Func는 C# 형식의 대리자임.
public class UnityEventSample : MonoBehaviour
{
    public UnityEvent onSample;

    private void Update()
    {
        //onSample에 실행할 기능이 등록된 상태에서 A키를 누르면
        if (Input.GetKeyDown(KeyCode.A) && onSample != null)
        {
            onSample.Invoke();
        }
    }
}
