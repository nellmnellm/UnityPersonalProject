using UnityEngine;

//singleton pattern
//�ϳ��� ���α׷� ���� ������ ��ü�� ����. 
//[������]
//�������� �׽�Ʈ�� �����ϴµ��� ������.
//�̱��� �ν��Ͻ��� ����Ǹ� �����ϴ� ��� Ŭ������ ������ ����.
//����� �����. 
//[����]
//�޸� ����� �پ��
public class GameManager : MonoBehaviour
{
    //1 class�� instance
    private static GameManager instance;

    //2. ������Ƽ ����
    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    //3. ����� �ʵ� �� �����
    
    public int value = 1;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //����� �ν��Ͻ��� �ڽ��� �ʱ�ȭ
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
