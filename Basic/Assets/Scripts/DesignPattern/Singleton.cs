using UnityEngine;


//T Singleton, MonoSingleton
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour   
{
    private static T _instance;

    public static T Instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>(); // ������ ã��. ������ ������

                if (_instance == null)
                {
                    GameObject go = new GameObject(); // �� ������Ʈ ����
                    go.name = typeof(T).Name; //�������� ���·� �̸� ����
                    go.AddComponent<T>(); //���� ������Ʈ�� ������Ʈ �߰�.
                    DontDestroyOnLoad(go); // ����ȯ�� �ı����� �ʵ���.
                }
            }
            return _instance; 
        }
        
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T; // this������ T �����̴�.
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(_instance);
        }
    }
}
