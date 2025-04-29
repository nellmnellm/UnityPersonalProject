using UnityEngine;
using UnityEngine.Events;

//�븮�� ��� �� �ϳ�.
//Action�̳� Func�� C# ������ �븮����.
public class UnityEventSample : MonoBehaviour
{
    public UnityEvent onSample;

    private void Update()
    {
        //onSample�� ������ ����� ��ϵ� ���¿��� AŰ�� ������
        if (Input.GetKeyDown(KeyCode.A) && onSample != null)
        {
            onSample.Invoke();
        }
    }
}
