using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour
{
    public List<Collider> targetList;

    private void Awake()
    {
        targetList = new List<Collider>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Remove(other);   
    }

    private void LateUpdate()
    {
        targetList.RemoveAll(target => target == null);
        //����Ʈ ���� => Contains(value) ���� ���Եǰ� �ִ���
        //Remove(value)�ش� ���� ����
        //Add(value)�ش簪�� �߰�(����Ʈ ��������)
        //RemoveAll(Predicate<T>) => ���� ������ �븮�ڸ� �ְ� ������ ����.
    }
}
