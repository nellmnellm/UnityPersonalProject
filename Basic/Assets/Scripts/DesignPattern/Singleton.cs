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
                _instance = FindAnyObjectByType<T>(); // 씬에서 찾기. 성능이 나빠짐

                if (_instance == null)
                {
                    GameObject go = new GameObject(); // 빈 오브젝트 생성
                    go.name = typeof(T).Name; //넣으려는 형태로 이름 생성
                    go.AddComponent<T>(); //게임 오브젝트에 컴포넌트 추가.
                    DontDestroyOnLoad(go); // 씬전환에 파괴되지 않도록.
                }
            }
            return _instance; 
        }
        
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T; // this이지만 T 형식이다.
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(_instance);
        }
    }
}
