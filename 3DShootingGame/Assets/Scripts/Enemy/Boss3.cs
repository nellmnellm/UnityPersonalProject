using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UniVRM10;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Boss3 : Enemy, IBoss
{
    private int phase = 1;
    private Coroutine phaseRoutine;
    [Header("다른 프리펩 총알")]
    public GameObject bulletPrefab2; // 파랑. 원래꺼는 빨강 설정
    public GameObject bulletPrefab3;
    public int initialPoolSize2 = 800;    //** 총알의 개수
    public int initialPoolSize3 = 800;
    protected List<GameObject> bulletObjectPool2 = new List<GameObject>(); //** 오브젝트 풀
    protected List<GameObject> bulletObjectPool3 = new List<GameObject>();
    private Animator animator;
    private bool isDead = false;

    private EnemySpawner nextBossSpawner;

    //hp UI 관련
    private int maxHP = 0;
    public float HPNormalized => (float)HP / maxHP;
    public Sprite hpBarSprite;
    public Sprite HPBarSprite => hpBarSprite;
    protected override void Awake()
    {
        base.Awake();
        StoryUIManager.Instance.ShowStory(StoryType.Intro);
        for (int i = 0; i < initialPoolSize2; i++)
        {
            AddBulletToPool(bulletPrefab2, bulletObjectPool2);
            AddBulletToPool(bulletPrefab3, bulletObjectPool3);
        }

    }

    private void Start()
    {

        animator = GetComponent<Animator>();
        speed = 5;
        dir = Vector3.down;

        enemyScore = 30000;

        var UI = GameObject.Find("BossHP");
        if (UI != null)
        {
            var barUI = UI.GetComponent<BossHPBar>();
            barUI.SetBoss(this);
        }
        StartPhase(phase); 
        StartCoroutine(AfterStop(1f));
    }

    private void StartPhase(int newPhase)
    {
        // 이전 패턴 종료
        if (phaseRoutine != null) StopCoroutine(phaseRoutine);

        phase = newPhase;

        switch (phase)
        {
            case 1:
                HP = 250;
                maxHP = 250;
                phaseRoutine = StartCoroutine(Phase1Pattern());
                break;

            case 2:
                HP = 300;
                maxHP = 300;
                animator.SetInteger("Phase", 2);
                phaseRoutine = StartCoroutine(Phase2Pattern());
                break;

            case 3:
                HP = 250;
                maxHP = 250;
                animator.SetInteger("Phase", 3);
                phaseRoutine = StartCoroutine(Phase3Pattern());
                break;

            default:
                Debug.Log("보스 페이즈 종료!");
                break;
        }
    }

    // === Phase 1 ===
    private IEnumerator Phase1Pattern()
    {
        while (true)
        {

            Vector3 randomPos = new Vector3(Random.Range(-7f, 7f), Random.Range(3f, 7f), 0);
            yield return StartCoroutine(MoveBossToPosition(randomPos, 1f));
            yield return StartCoroutine(FireEarthwormBullets());
        }
    }

    


    private IEnumerator FireEarthwormBullets()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 PlayerDir = (target.transform.position - firePoint.position).normalized;

            float radius = 3f;


            for (int i = 0; i < 240; i++)
            {
                Vector3 spawnPoint = transform.position + radius * (Quaternion.Euler(0, 0, 12f * i) * Vector3.down);
                Vector3 spawnPoint2 = transform.position + radius * (Quaternion.Euler(0, 0, -12f * i) * Vector3.down);
                float startTime = Time.time;
                Vector3 bulletDir = (target.transform.position - spawnPoint).normalized;
                SetBullet(bulletObjectPool, spawnPoint, () => bulletDir, () => 5 * (Time.time - startTime) * (Time.time - startTime));
                SetBullet(bulletObjectPool2, spawnPoint2, () => bulletDir, () => 5 * (Time.time - startTime) * (Time.time - startTime));
                yield return new WaitForSeconds(0.01f);
            }

        }

        yield return null;

    }


   
    // === Phase 2 ===

    private void CreateBulletSpawnPoints(float speed)
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-8, 11, 0),
            new Vector3(-8,  -1, 0),
            new Vector3(-8,  5, 0),
            new Vector3(-8, -7, 0),
            new Vector3(-8, -13, 0),
        };
        Vector3[] ReversePositions = new Vector3[]
        {
            new Vector3(8, 8, 0),
            new Vector3( 8,  -4, 0),
            new Vector3( 8,  2, 0),
            new Vector3( 8, -10, 0),
            new Vector3( 8, -16, 0)
        };

        

        for (int i = 0; i < positions.Length; i++)
        {
            float startTime = Time.time;
            int currentI = i; // -n, 0, n도
            SetBullet(bulletObjectPool2, firePoint.position + positions[i],
                () => Quaternion.Euler(0, 0, (Time.time - startTime) * -10) * Vector3.right, () => speed);
            SetBullet(bulletObjectPool2, firePoint.position + ReversePositions[i],
                () => Quaternion.Euler(0, 0, (Time.time - startTime) * -10) * Vector3.left, () => speed);
        }


    }


    private IEnumerator Phase2Pattern()
    {
        yield return StartCoroutine(MoveBossToPosition(new Vector3(0, 4, 0), 1f));
        yield return new WaitForSeconds(2f);
        float startTime = Time.time;
        
        while (true)
        {
            StartCoroutine(FireBloomPattern(startTime));
            yield return new WaitForSeconds(1f);
        }
}


    private IEnumerator FireBloomPattern(float startTime)
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            for (int i = 0; i < 50; i++)
            {
                int currentI = i;
                float angle = 7.2f * currentI;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * bulletDir;

                // 밖으로 퍼지는 탄환
                SetBullet(bulletObjectPool, firePoint.position, 
                    () => Quaternion.Euler(0, 0, 3 * (Time.time - startTime)) * dir , 
                    () => 5f);

                
                SetBullet(bulletObjectPool2, firePoint.position + dir * 2.2f, 
                    () => Quaternion.Euler(0, 0, - 3 * (Time.time - startTime)) * dir,
                    () => 5f);

                SetBullet(bulletObjectPool3, firePoint.position + dir * 3.8f,
                    () => dir,
                    () => 5f);
            }

            yield return new WaitForSeconds(1.3f);
        }
    }



    // === Phase 3 ===
    private IEnumerator Phase3Pattern()
    {
        yield return new WaitForSeconds(2.5f);
        while (true)
        {
            // 1. 랜덤 위치로 이동
            Vector3 randomPos = new Vector3(Random.Range(-7f, 7f), Random.Range(3f, 7f), 0);
            yield return StartCoroutine(MoveBossToPosition(randomPos, 1f));

            // 2. 패턴 실행
            yield return StartCoroutine(FireFanBullets());
            yield return new WaitForSeconds(0.5f);
        }
    }

    


    private IEnumerator FireFanBullets()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 PlayerDir = (target.transform.position - firePoint.position).normalized;
            Vector3 perp = new Vector3(-PlayerDir.y, PlayerDir.x, 0); // 수직 벡터

            // 5개의 스폰 포인트 계산 (-2 ~ +2 오프셋)
            for (int i = -2; i <= 2; i++)
            {
                Vector3 spawnPoint = transform.position + perp * i;

                // 속도 3~10 사이의 총알 8개 생성
                for (int j = 0; j < 8; j++)
                {
                    Vector3 bulletDir = (target.transform.position - spawnPoint).normalized;
                    float speed = Mathf.Lerp(3f, 17f, 2 * j / 14f); // 선형 보간
                    SetBullet(bulletObjectPool, spawnPoint, () => bulletDir, () => speed);
                    SetBullet(bulletObjectPool2, spawnPoint, () => Quaternion.Euler(0, 0, 22) * bulletDir, () => speed);
                    SetBullet(bulletObjectPool3, spawnPoint, () => Quaternion.Euler(0, 0, -22) * bulletDir, () => speed);
                }
            }

            yield return null;
        }
    }

    

    // === 페이즈 체력 처리 ===
    protected new void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // 이미 죽었으면 처리 안 함

        if (other.CompareTag("Bullet"))
        {
            HP--;

            if (HP <= 0)
            {
                HandleDeath();
            }

            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Bomb"))
        {
            HP -= 50;

            if (HP <= 0)
            {
                HandleDeath();
            }
        }
    }

    private void HandleDeath()
    {
        if (isDead) return;

        isDead = true;

        if (phase < 3)
        {
            StartPhase(phase + 1);
            isDead = false; // 다음 페이즈 시작 후 다시 죽을 수 있음
        }
        else
        {
            ScoreManager.Instance.Score += enemyScore;
            var explosion = Instantiate(effect, transform.position, Quaternion.identity);
            
            //준보스기 때문에 스포너 연결. 이름에 조심할것.
            var spawner = GameObject.Find("Boss4Spawner")?.GetComponent<EnemySpawner>();
            if (spawner != null)
                GameObject.Find("EnemySpawnManager")?.GetComponent<EnemySpawnManager>().ForcedSpawner(spawner);
            ClearBulletPool();
            Destroy(gameObject);
            
        }
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
        foreach (var bullet in bulletObjectPool2)
        {
            if (bullet != null)
            {
                Destroy(bullet); // 실 메모리에서 제거
            }
        }
        foreach (var bullet in bulletObjectPool3)
        {
            if (bullet != null)
            {
                Destroy(bullet); // 실 메모리에서 제거
            }

        }

    }


    private IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }


}