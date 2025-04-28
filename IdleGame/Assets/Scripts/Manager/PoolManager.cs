using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    // 1. ��������� ��ġ
    Transform Trans { get; set; }
    // 2. Ǯ �ȿ� ������� ���� ������Ʈ
    Queue<GameObject> pool { get; set; }
    // 3. Ǯ���� ������ �۾�
    GameObject get(Action<GameObject> action = null);
    // ����Ƽ Action �븮�� Action, Func, Delegate(C#����), UnityEvent(ex. OnClick)
    // Action : void ������ �Լ� ó���� ������ ��
    // Func : return ���� �ִ� ������ �Լ� ó���� ������ ��.
    // �Ű������� ���� ���س����� default param. 

    // 4. Ǯ�� ���� return (����)
    void Release(GameObject obj, Action<GameObject> action = null);
}

public class PoolManager : MonoBehaviour
{
    //��ųʸ� => Ű�� ���� �����ϱ� ���� ������ ��. ���� �������� data(�ߺ�����)
    public Dictionary<string, IPool> pool_dict = new Dictionary<string, IPool>();
    Transform b_obj;

    public IPool pooling(string path)
    {
        //ContainsKey(key) key�� ��ųʸ��� �ִ��� �Ǵ�
        if (pool_dict.ContainsKey(path) == false)
        {
            Add_pool(path);
        }
        //Ǯ�� ��ϵ� ���� ���� ���
        if (pool_dict[path].pool.Count <= 0)
        { 
            Add_Queue(path);
        }
        return pool_dict[path];

    }
    /// <summary>
    /// Ǯ ���
    /// </summary>
    /// <param name="path">���</param>
    
    private GameObject Add_pool(string path)
    {
        var obj = new GameObject($"{path} Pool");
        //��θ�+pool�� �� ������Ʈ�� ����.
        var obj_pool = new ObjectPool();

        pool_dict.Add(path, obj_pool);
        obj_pool.Trans = obj.transform;
        return obj;
    }
    
    /// <summary>
    /// ť�� �߰�
    /// </summary>
    /// <param name="path">���</param>
    private void Add_Queue(string path)
    {
        //��η� ���� ������Ʈ�� ����
        //var obj = Instantiate(Resources.Load<GameObject>(path)); => �Ʒ��ٿ� �̱��� ���
        var obj = Manager.Instance.ResourceInstantiate(path); 
        // Resources.Load<T>()�� ����Ƽ ������Ʈ ���� Resources �����κ���
        // ��ο� �´� <T>�� �ε��ϴ� ���. 
        obj.transform.parent = pool_dict[path].Trans;
        pool_dict[path].Release(obj);

        //obj.SetActive(false);

    }
}
