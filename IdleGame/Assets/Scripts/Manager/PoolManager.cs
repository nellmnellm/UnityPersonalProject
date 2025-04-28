using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    // 1. 만들어지는 위치
    Transform Trans { get; set; }
    // 2. 풀 안에 만들어줄 게임 오브젝트
    Queue<GameObject> pool { get; set; }
    // 3. 풀에서 얻어오는 작업
    GameObject get(Action<GameObject> action = null);
    // 유니티 Action 대리자 Action, Func, Delegate(C#전용), UnityEvent(ex. OnClick)
    // Action : void 형태의 함수 처리를 진행할 때
    // Func : return 값이 있는 형태의 함수 처리를 진행할 때.
    // 매개변수에 값을 전해놓으면 default param. 

    // 4. 풀에 대한 return (해제)
    void Release(GameObject obj, Action<GameObject> action = null);
}

public class PoolManager : MonoBehaviour
{
    //딕셔너리 => 키는 값을 접근하기 위한 고유한 값. 값은 실질적인 data(중복가능)
    public Dictionary<string, IPool> pool_dict = new Dictionary<string, IPool>();
    Transform b_obj;

    public IPool pooling(string path)
    {
        //ContainsKey(key) key가 딕셔너리에 있는지 판단
        if (pool_dict.ContainsKey(path) == false)
        {
            Add_pool(path);
        }
        //풀에 등록된 값이 없는 경우
        if (pool_dict[path].pool.Count <= 0)
        { 
            Add_Queue(path);
        }
        return pool_dict[path];

    }
    /// <summary>
    /// 풀 등록
    /// </summary>
    /// <param name="path">경로</param>
    
    private GameObject Add_pool(string path)
    {
        var obj = new GameObject($"{path} Pool");
        //경로명+pool로 빈 오브젝트가 생성.
        var obj_pool = new ObjectPool();

        pool_dict.Add(path, obj_pool);
        obj_pool.Trans = obj.transform;
        return obj;
    }
    
    /// <summary>
    /// 큐에 추가
    /// </summary>
    /// <param name="path">경로</param>
    private void Add_Queue(string path)
    {
        //경로로 게임 오브젝트를 생성
        //var obj = Instantiate(Resources.Load<GameObject>(path)); => 아래줄에 싱글톤 기반
        var obj = Manager.Instance.ResourceInstantiate(path); 
        // Resources.Load<T>()는 유니티 프로젝트 내의 Resources 폴더로부터
        // 경로에 맞는 <T>를 로드하는 기능. 
        obj.transform.parent = pool_dict[path].Trans;
        pool_dict[path].Release(obj);

        //obj.SetActive(false);

    }
}
