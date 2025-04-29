using UnityEngine;

//singleton pattern
//하나의 프로그램 내의 유일한 객체를 만듬. 
//[문제점]
//독립적인 테스트를 진행하는데는 부적합.
//싱글톤 인스턴스가 변경되면 참조하는 모든 클래스의 수정이 진행.
//상속이 어려움. 
//[장점]
//메모리 사용이 줄어듬
public class GameManager : MonoBehaviour
{
    //1 class명 instance
    private static GameManager instance;

    //2. 프로퍼티 접근
    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    //3. 사용할 필드 값 만들기
    
    public int value = 1;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //사용할 인스턴스에 자신을 초기화
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public class Test
    {
        void Use()
        {
            GameManager.Instance.value++;
        }
    }

    public class BasicSingleton
    {
        private static BasicSingleton instance;

        private BasicSingleton()
        {
        }

        public static BasicSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new BasicSingleton();
            }
            return instance;
        }
    }
}
