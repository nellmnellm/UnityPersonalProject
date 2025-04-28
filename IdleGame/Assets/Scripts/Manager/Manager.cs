using System;
using System.Collections.Generic;
using UnityEngine;


//매니저들을 관리하는 매니저
public class Manager : MonoBehaviour
{
    #region Singleton
    //자기 자신에 대한 전역 인스턴스를 필드 또는 프로퍼티 가짐.
    // 기본값은 null.

    public static Manager Instance = null;

    //게임이 시작되기 전 초기화

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //안전지대 생성. 씬으로 구성됨
            //게임오브젝트를 DDOL로 넘겨줌. 씬을 로드해도 파괴안되고 그대로 전달.
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //등록된 매니져들을 접근가능하도록 
    private static PoolManager PoolManager = new PoolManager();

    //매니저 접근을 위한 프로퍼티
    public static PoolManager Pool { get { return PoolManager; } }


    public GameObject ResourceInstantiate(string path) =>
        Instantiate(Resources.Load<GameObject>(path));


}
