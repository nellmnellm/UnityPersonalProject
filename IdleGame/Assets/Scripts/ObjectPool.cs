using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : IPool
{
    public Transform Trans { get; set; }
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public GameObject get(Action<GameObject> action = null)
    {
        var obj = pool.Dequeue(); //풀에서 하나 빼옴
        obj.SetActive(true); // 게임 오브젝트의 활성화

        // Action으로 실행할 기능이 있다면 
        if (action != null)
        {
            action.Invoke(obj); // ?는 멀티스레드환경 에서 
        }
        return obj;
    }

    public void Release(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj); //풀에 오브젝트를 등록함.
        obj.transform.parent = Trans; //부모 트랜스폼에 등록
        obj.SetActive(false);
        
        // Action으로 실행할 기능이 있다면 
        if (action != null)
        {
            action?.Invoke(obj); // ?는 멀티스레드환경 에서 
        }


    }
}
