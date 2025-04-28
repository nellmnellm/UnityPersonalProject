using System;
using System.Collections.Generic;
using UnityEngine;


//�Ŵ������� �����ϴ� �Ŵ���
public class Manager : MonoBehaviour
{
    #region Singleton
    //�ڱ� �ڽſ� ���� ���� �ν��Ͻ��� �ʵ� �Ǵ� ������Ƽ ����.
    // �⺻���� null.

    public static Manager Instance = null;

    //������ ���۵Ǳ� �� �ʱ�ȭ

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //�������� ����. ������ ������
            //���ӿ�����Ʈ�� DDOL�� �Ѱ���. ���� �ε��ص� �ı��ȵǰ� �״�� ����.
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //��ϵ� �Ŵ������� ���ٰ����ϵ��� 
    private static PoolManager PoolManager = new PoolManager();

    //�Ŵ��� ������ ���� ������Ƽ
    public static PoolManager Pool { get { return PoolManager; } }


    public GameObject ResourceInstantiate(string path) =>
        Instantiate(Resources.Load<GameObject>(path));


}
