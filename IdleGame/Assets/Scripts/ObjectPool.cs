using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : IPool
{
    public Transform Trans { get; set; }
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public GameObject get(Action<GameObject> action = null)
    {
        var obj = pool.Dequeue(); //Ǯ���� �ϳ� ����
        obj.SetActive(true); // ���� ������Ʈ�� Ȱ��ȭ

        // Action���� ������ ����� �ִٸ� 
        if (action != null)
        {
            action.Invoke(obj); // ?�� ��Ƽ������ȯ�� ���� 
        }
        return obj;
    }

    public void Release(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj); //Ǯ�� ������Ʈ�� �����.
        obj.transform.parent = Trans; //�θ� Ʈ�������� ���
        obj.SetActive(false);
        
        // Action���� ������ ����� �ִٸ� 
        if (action != null)
        {
            action?.Invoke(obj); // ?�� ��Ƽ������ȯ�� ���� 
        }


    }
}
