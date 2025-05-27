using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    #region singleton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCam = Camera.main;
        playerAnimator = GetComponent<Animator>();
    }

    #endregion

    [Header("총알 관리, 발사 간격")]
    [SerializeField] private float fireInterval = 0.14f; //발사 간격
    private float fireCooldownTimer = 0f;
    public GameObject bulletPrefab;       // bullet prefab 할당

    [Header("목숨 관리")]
    public int HP = 5;                    // 플레이어의 목숨 수
    public GameObject heartImagePrefab;             // 목숨 이미지 프리펩
    public List<GameObject> hearts = new List<GameObject>(5);
    public Transform heartContainer;      // 목숨 UI가 들어갈 위치
    
    [Header("레벨 관리")]
    public int level = 1;                  // 총알 개수 (아직 구현 안된 부분 => 3 고정)

    [Header("폭탄 관리")]
    
    public int bombCount = 2;
    public GameObject bombImagePrefab;
    public List<GameObject> bombs = new List<GameObject>(2);
    public Transform bombContainer;
    
    public GameObject bombPrefab;
    public float effectDuration = 2.0f;
    public float bombCooltime = 5f;
    private float lastBombTime = -999f;

    [Header("폭탄 컷씬 관리")]
    public Camera mainCam;
    public Camera cutsceneCam;
    private Animator playerAnimator;
    public float cutsceneTime = 1.0f;
    [Header("피격시 무적 조절")]
    public GameObject effect;              // 피격시 이펙트
    // public float invincibleDuration = 2f;  // 피격시 무적 시간
    // => 같은 IEnumerator를 재활용해서 5초 무적 부여하기 위해 코루틴 내부 파라미터로 선언. 
    
    private bool isInvincible = false;     // 반짝이는 효과 적용 여부
    private Renderer[] renderers;          // 반짝이는 효과 적용을 위한 renderer 집합(구조의 하위구조)
                  // -> 무적시간 등을 위해. 인스펙터에서 직접

    public GameObject[] firePosition;      //

    [Header("오브젝트 풀")]
    public int initialPoolSize = 100;    //** 총알의 개수
    public List<GameObject> bulletObjectPool = new List<GameObject>(); //** 오브젝트 풀

    
    public GameObject AddBulletToPool()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletObjectPool.Add(bullet);
        return bullet;
    }

    public void ClearBulletPool()
    {
        foreach (var bullet in bulletObjectPool)
        {
            if (bullet != null)
            {
                Destroy(bullet); // 실 메모리에서 제거
            }
        }
        bulletObjectPool.Clear();
    }

    public void UpdateBombs()
    {
        if (bombs.Count != bombCount)
        {
            // 기존 하트 제거
            foreach (var bomb in bombs)
            {
                Destroy(bomb);
            }
            bombs.Clear();

            // 새로운 하트 생성
            for (int i = 0; i < bombCount; i++)
            {
                GameObject bomb = Instantiate(bombImagePrefab, bombContainer);
                bombs.Add(bomb);
            }
        }
    }
    public void UpdateHearts()
    {
        
        // 하트 수가 다르면 다시 생성
        if (hearts.Count != HP)
        {
            // 기존 하트 제거
            foreach (var heart in hearts)
            {
                Destroy(heart);
            }
            hearts.Clear();
            if (HP >= 6)
            {
                return;
            }
            else
            {
                // 새로운 하트 생성
                for (int i = 0; i < HP; i++)
                {
                    GameObject heart = Instantiate(heartImagePrefab, heartContainer);
                    hearts.Add(heart);
                }
            }
                
                
        }
    }
    //씬 이동시 사용
    public void SetUIContainers(Transform newHeartContainer, Transform newBombContainer)
    {
        heartContainer = newHeartContainer;
        bombContainer = newBombContainer;
        UpdateHearts();
        UpdateBombs();
    }

    private void Start()
    {
        mainCam = Camera.main;
        cutsceneCam.enabled = false;
        playerAnimator = GetComponent<Animator>();

        UpdateHearts();
        UpdateBombs();
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddBulletToPool();
        }
        // 모든 자식의 Renderer (몸이 여러 MeshRenderer로 이루어져 있을 수 있음)
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        fireCooldownTimer -= Time.deltaTime;

        if (Input.GetButton("Fire1") && fireCooldownTimer <= 0f && bulletObjectPool.Count > 0
            && GetComponent<Collider>().enabled == true)
        {
            for (int j = 0; j < level; j++)
            {
                for (int i = 0; i < initialPoolSize; i++) // 오브젝트 풀
                {
                    var bullet = bulletObjectPool[i];

                    if (!bullet.activeSelf)
                    {
                        bullet.SetActive(true);
                        bullet.transform.position = firePosition[j].transform.position;
                        break;
                    }
                }
            }

            fireCooldownTimer = fireInterval; // 쿨타임 초기화
        }

        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            if (Time.time - lastBombTime < bombCooltime)
            {
                StartCoroutine(ShakeCamera());
                return;
            }
            UseBomb();
            bombCount--;
            UpdateBombs();
            lastBombTime = Time.time;
        }
    }

    public void UseBomb()
    {
        StartCoroutine(BombCutsceneRoutine());
    }

    IEnumerator BombCutsceneRoutine()
    {
        var collider = GetComponent<Collider>();
        // 컷씬 카메라 활성화
        mainCam.enabled = false;
        cutsceneCam.enabled = true;
        collider.enabled = false;
        //transform.position = new Vector3(0, -50, 0);
        // 애니메이션 트리거
        playerAnimator.SetTrigger("Punch");

        // 컷씬 유지
        yield return new WaitForSecondsRealtime(cutsceneTime);

        // 카메라 복귀
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        collider.enabled = true;
        //transform.position = new Vector3(0, -5, 0);
        StartCoroutine(InvincibilityCoroutine(2f));
        // 폭탄 효과 시작
        GameObject bomb = Instantiate(bombPrefab, new Vector3(0, -1f, -0.1f), Quaternion.identity);
        bomb.SetActive(true);
        
    }
    IEnumerator ShakeCamera(float duration = 0.2f, float magnitude = 0.1f)
    {
        Vector3 originalPos = mainCam.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            mainCam.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.localPosition = originalPos;
    }
    #region 
    /* private void Update()
     {
         if (Input.GetButtonDown("Fire1"))
         {
             for (int j=0; j<level; j++)
             {
                 for (int i = 0; i < initialPoolSize; i++)              //** 오브젝트풀
                 {
                     var bullet = bulletObjectPool[i];

                     if (bullet.activeSelf == false)
                     {
                         bullet.SetActive(true);

                         bullet.transform.position = firePosition[j].transform.position;

                         break;

                     }
                 }
             }
         }

        *//* if (Input.GetButtonDown("Fire1"))
         {

             for (int i=0; i<level; i++)
             {
                 var bullet = Instantiate(bulletFactory);
                 bullet.transform.position = firePosition[i].transform.position;

             }

         }*//*
     }*/


    /*  private void OnCollisionEnter(Collision collision)
      {
          if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
          {
              TakeDamage(1);
          }

          else if (collision.gameObject.CompareTag("Item"))
          {
              if (level < 5)
                  level++;
              //else
                  //score += 100;
          }
      }*/
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Enemy") && !isInvincible)
            || (other.gameObject.CompareTag("EnemyBullet") && !isInvincible))
        {
            TakeDamage(1);
        }

        else if (other.gameObject.CompareTag("Item"))
        {
            if (level < 5)
                level++;
            //else
            //score += 100;
        }
    }
    void TakeDamage(int damage)
    {
        
        HP -= damage;
        bombCount = 0;
        UpdateHearts();
        var explosion = Instantiate(effect);
        explosion.transform.position = transform.position;
        if (HP <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnAfterDelay(1f));
        }


    }

    void GameOver()
    {
        ScoreManager.Instance.ShowGameOverPanel();
    }
   

    IEnumerator RespawnAfterDelay(float delay)
    {
        var collider = GetComponent<Collider>();
        // 렌더링 & 충돌 꺼서 안 보이게 함
        collider.enabled = false;
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = false;
        }
        //transform.position = new Vector3(0f, -50f, 0f);
        yield return new WaitForSeconds(delay);
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = true;
        }
        bombCount = 2;
        UpdateBombs();
        collider.enabled = true;
        transform.position = new Vector3(0f, -5f, 0f);
        StartCoroutine(InvincibilityCoroutine(4f));
    }

    IEnumerator InvincibilityCoroutine(float invincibleDuration)
    {
        

        float blinkInterval = 0.2f;
        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
            isInvincible = true;
            SetRenderersVisible(false);
            yield return new WaitForSeconds(blinkInterval);
            SetRenderersVisible(true);
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2;
        }

        isInvincible = false;
    }

    void SetRenderersVisible(bool visible)
    {
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = visible;
        }
    }

}