using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
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
    public float invincibleDuration = 2f;  // 피격시 무적 시간
    private bool isInvincible = false;     // 반짝이는 효과 적용 여부
    private Renderer[] renderers;          // 반짝이는 효과 적용을 위한 renderer 집합(구조의 하위구조)
    

    public GameObject[] firePosition;      //

    [Header("오브젝트 풀")]
    public int initialPoolSize = 100;    //** 총알의 개수
    public List<GameObject> bulletObjectPool = new List<GameObject>(); //** 오브젝트 풀


    private GameObject AddBulletToPool()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletObjectPool.Add(bullet);
        return bullet;
    }
    void UpdateBombs()
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
    void UpdateHearts()
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

            // 새로운 하트 생성
            for (int i = 0; i < HP; i++)
            {
                GameObject heart = Instantiate(heartImagePrefab, heartContainer);
                hearts.Add(heart);
            }
        }
    }

    void Start()
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

        if (Input.GetButton("Fire1") && fireCooldownTimer <= 0f)
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
        // 컷씬 카메라 활성화
        mainCam.enabled = false;
        cutsceneCam.enabled = true;

        transform.position += new Vector3(0, 0, 2);
        // 애니메이션 트리거
        playerAnimator.SetTrigger("Punch");

        // 컷씬 유지
        yield return new WaitForSecondsRealtime(cutsceneTime);

        // 카메라 복귀
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        StartCoroutine(InvincibilityCoroutine());
        // 폭탄 효과 시작
        GameObject bomb = Instantiate(bombPrefab, new Vector3(0, -1f, -0.1f), Quaternion.identity);
        bomb.SetActive(true);
        transform.position += new Vector3(0, 0, -2);
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
        UpdateHearts();
        bombCount = 2;
        UpdateBombs();
        var explosion = Instantiate(effect);
        explosion.transform.position = transform.position;

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float blinkInterval = 0.2f;
        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
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