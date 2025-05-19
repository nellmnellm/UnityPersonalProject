using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UniVRM10;
using Random = UnityEngine.Random;

public class Boss1 : Enemy
{
    private int phase = 1;
    private Coroutine phaseRoutine;
    [Header("다른 프리펩 총알")]
    public GameObject bulletPrefab2; // 파랑. 원래꺼는 빨강 설정
    public int initialPoolSize2 = 800;    //** 총알의 개수
    protected List<GameObject> bulletObjectPool2 = new List<GameObject>(); //** 오브젝트 풀

    private Animator animator;
    private bool isDead = false;

    //hp UI 관련
    private int maxHP = 0; 
    public float HPNormalized => (float)HP / maxHP;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < initialPoolSize2; i++)
        {
            AddBulletToPool(bulletPrefab2, bulletObjectPool2);
        }
    
    }
    
    private void Start()
    {
        
        animator = GetComponent<Animator>();
        speed = 5;
        dir = Vector3.down;
        
        enemyScore = 10000;

        var UI = GameObject.Find("BossHP");
        if (UI != null)
        {
            var barUI = UI.GetComponent<BossHPBar>();
            barUI.SetBoss(this);
        }
        
        StartCoroutine(AfterStop(1f));
        StartPhase(phase); // 첫 페이즈 시작


    }

    private void StartPhase(int newPhase)
    {
        // 이전 패턴 종료
        if (phaseRoutine != null) StopCoroutine(phaseRoutine);
       
        phase = newPhase;

        switch (phase)
        {
            case 1:
                HP = 180;
                maxHP = 180;
                phaseRoutine = StartCoroutine(Phase1Pattern());
                break;

            case 2:
                HP = 300;
                maxHP = 300;
                animator.SetInteger("Phase", 2);
                phaseRoutine = StartCoroutine(Phase2Pattern());
                break;

            case 3:
                HP = 400;
                maxHP = 400;
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
        int count = 0;
        while (true)
        {
            FireBullet1phase();

            if (count % 25 == 0)
            {
                StartCoroutine(FireRadialDelayed());
            }

            count++;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FireBullet1phase()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            

            for (int i = 5; i < 10; i++)
            {
                
                for (int j = 0; j < 6; j++)
                {
                    Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
                    int currentI = i; // C# 클로저 문제! 람다함수가 참조만 하다가 마지막에 호출하는데 9만 반영됨. 나중에 더 알아볼것.
                    int currentJ = j;
                    SetBullet(bulletObjectPool, firePoint.position, () => Quaternion.Euler(0, 0, currentJ * 60) * bulletDir, () => 2.4f * currentI);
                    //CreateBullet(bulletPrefab, firePoint.position, () => Quaternion.Euler(0, 0, currentJ * 60) * bulletDir, () => 2.4f * currentI); 
                    //CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, j * 60) * bulletDir, () => 2.4f * i);
                }
            }

            
            
        }
    }

    private IEnumerator FireRadialDelayed()
    {
        Vector3 center = transform.position; // 보스의 위치
        int bulletCount = 16;

        List<GameObject> bullets = new List<GameObject>();

        //  경계에 총알 생성 (원형)
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad) * 4f, Mathf.Sin(rad) * 4f, 0f); // 반지름 15

            Vector3 spawnPos = center + offset;
            Vector3 targetDir = (center - spawnPos).normalized;

            GameObject bullet = Instantiate(bulletPrefab2, spawnPos, Quaternion.identity);
            var bulletComp = bullet.GetComponent<EnemyBullet>();

            bulletComp.SetDirection(() => targetDir);
            bulletComp.SetSpeed(() => 0f); // 초기속도 0

            bullets.Add(bullet);
        }

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 점점 빨라지는 속도 적용
        float startTime = Time.time;

        foreach (var bullet in bullets)
        {
            var bulletComp = bullet.GetComponent<EnemyBullet>();

            if (bulletComp != null)
            {
                bulletComp.SetSpeed(() => (Time.time - startTime) * 10f);
            }
        }
    }


    // === Phase 2 ===

    private void CreateBulletSpawnPoints(float angle, float speed)
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-8,  -2, 0),
            new Vector3(-8,  4, 0),
            new Vector3(-8, -8, 0),
        };
        Vector3[] ReversePositions = new Vector3[]
        {
            new Vector3( 8,  -2, 0),
            new Vector3( 8,  4, 0),
            new Vector3( 8, -8, 0)
        };

        

        for (int i = 0; i < positions.Length; i++)
        {
            int currentI = i;
            for (int j = 0; j < 3; j++)
            {
                //int currentJ = j;
                // n도씩 퍼지는 3발
                float angle3 = -angle + (angle * j); // -n, 0, n도
                SetBullet(bulletObjectPool, firePoint.position + positions[i],
                    () => Quaternion.Euler(0, 0, angle3) * Vector3.right, () => speed);
                SetBullet(bulletObjectPool, firePoint.position + ReversePositions[i],
                    () => Quaternion.Euler(0, 0, angle3) * Vector3.left, () => speed);
               /* CreateBullet(bulletPrefab, firePoint.position + positions[i], 
                    () => Quaternion.Euler(0, 0, angle3) * Vector3.right, () => speed);
                CreateBullet(bulletPrefab, firePoint.position + ReversePositions[i],
                    () => Quaternion.Euler(0, 0, angle3) * Vector3.left, () => speed);*/
            }
            
        }
       

    }


    private IEnumerator Phase2Pattern()
    {
        int count = 0;
        while (true)
        {
            if (count % 3 == 0)
            {
                CreateBulletSpawnPoints(21f, 2f);
            }
            count++;
            FireBulletPhase2();
            yield return new WaitForSeconds(0.17f);
        }
    }

    private void FireBulletPhase2()
    {
        // 보스 주변 원형 발사 예시
        int bulletCount = 18;
        float angle2 = Random.Range(0f, 360f);
        for (int i = 0; i < bulletCount; i++)
        {

            float angle = i * (360f / bulletCount);
            Vector3 dir = Quaternion.Euler(0, 0, angle + angle2) * Vector3.down;
            SetBullet(bulletObjectPool2, firePoint.position, () => dir, () => 8);
            //CreateBullet(bulletPrefab2, firePoint.position, () => dir, () => 8);
        }
    }
    
    // === Phase 3 ===
    private IEnumerator Phase3Pattern()
    {
        int count = 0;
        while (true)
        {
            if (count % 3 == 0)
            {
                CreateBulletSpawnPoints(45f, 4f);
            }
            count++;
            FireBulletPhase3();
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void FireBulletPhase3()
    {
        // 혼합 패턴: 랜덤 각도 + 빠른 속도
        for (int i = 0; i < 12; i++)
        {
            float angle = Random.Range(0f, 360f);
            SetBullet(bulletObjectPool2, firePoint.position, () => Quaternion.Euler(0, 0, angle) * Vector3.down, () => 8f);
            //CreateBullet(bulletPrefab2, firePoint.position, () => Quaternion.Euler(0, 0, angle) * Vector3.down, () => 8f);
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
            ScoreManager.instance.Score += enemyScore;
            var explosion = Instantiate(effect, transform.position, Quaternion.identity);
            // 여기서 Destroy 안 하고 애니메이션 실행
            animator.SetTrigger("Die");

            // 스토리 연출 호출
            StartCoroutine(WaitAndShowStory(3f));
        }
    }

    IEnumerator WaitAndShowStory(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StoryUIManager.Instance.ShowStory(); // 싱글톤 or 다른 방식
    }

   
    
    
    private IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }

    
}