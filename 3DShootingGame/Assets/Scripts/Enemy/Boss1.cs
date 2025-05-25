using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss1 : Enemy, IBoss
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
                HP = 200;
                maxHP = 200;
                phaseRoutine = StartCoroutine(Phase1Pattern());
                break;

            case 2:
                HP = 250;
                maxHP = 250;
                animator.SetInteger("Phase", 2);
                phaseRoutine = StartCoroutine(Phase2Pattern());
                break;

            case 3:
                HP = 300;
                maxHP = 300;
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
        yield return new WaitForSeconds(1f);
        int count = 0;
        while (true)
        {
            FireBullet1phase();

            if (count % 5 == 0)
            {
                StartCoroutine(FireStarDelayed());
            }

            count++;

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void FireBullet1phase()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            SetBullet(bulletObjectPool, firePoint.position, () => bulletDir, () => 15);
            SetBullet(bulletObjectPool, firePoint.position, () => Quaternion.Euler(0, 0, 120) * bulletDir, () => 15);
            SetBullet(bulletObjectPool, firePoint.position, () => Quaternion.Euler(0, 0, 240) * bulletDir, () => 15);
        }
    }

    private IEnumerator FireStarDelayed()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(5f, 10f);
        float randomAngle = Random.Range(0f, 72f);
        float spacing = 1f;
        Vector3 center = new Vector3(randomX, randomY, 0); // 탄의 처음 소환 위치
        float startTime = Time.time;
        //  경계에 총알 생성 (원형)
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float rad = (randomAngle + (72 * i)) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad) * spacing, Mathf.Sin(rad) * spacing, 0f); // 반지름 15
                int CurrentJ = j;
                Vector3 spawnPos = center + offset * (CurrentJ + 1);
                Vector3 targetDir = (center - spawnPos).normalized;

                SetBullet(bulletObjectPool2, spawnPos,
                    () => Quaternion.Euler(0, 0, 10 * (Time.time - startTime) * (Time.time - startTime)) * -targetDir,
                    () => 7 * (Time.time - startTime));
            }

        }

        yield return new WaitForSeconds(1f);
        float startTime2 = Time.time;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float rad = (randomAngle + (72 * i)) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad) * spacing, Mathf.Sin(rad) * spacing, 0f); // 반지름 15
                int CurrentJ = j;
                Vector3 spawnPos = center + offset * (CurrentJ + 1);
                Vector3 targetDir = (center - spawnPos).normalized;

                SetBullet(bulletObjectPool2, spawnPos,
                    () => Quaternion.Euler(0, 0, 10 * (Time.time - startTime) * (Time.time - startTime)) * -targetDir,
                    () => 7 * (Time.time - startTime2));
            }

        }

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
        int count = 0;
        while (true)
        {
            if (count < 11)
            {
                CreateBulletSpawnPoints(3f);
            }
            count++;
            if (count >= 40)
                count = 0;
            if (count % 8 == 0)
            {
                FireBulletPhase2();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FireBulletPhase2()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            float startTime = Time.time;
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            for (int i = 3; i < 12; i++)
            {
                int CurrentI = i;
                float angle = 360f / 12 * i;
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * 2; //5는 반경
                Vector3 bulletPos = firePoint.position + offset;

                SetBullet(bulletObjectPool, bulletPos,
                    () => Quaternion.Euler(0, 0, 5) * bulletDir,
                    () => 12);
            }
        }
    }

    // === Phase 3 ===
    private IEnumerator Phase3Pattern()
    {
        int count = 0;
        while (true)
        {
            if (count == 17)
            {
                StartCoroutine(FireLineDelayed());
            }

            if (count % 3 == 0)
            {
                FireBulletPhase3();
            }
            count++;
            if (count == 18)
            {
                count = 0;
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void FireBulletPhase3()
    {
        // 혼합 패턴: 랜덤 각도 + 빠른 속도
        for (int i = 0; i < 8; i++)
        {
            int CurrentI = i;
            float angle = Random.Range(0f, 360f);
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, angle) * Vector3.down,
                () => 8f);
            SetBullet(bulletObjectPool2, firePoint.position,
                () => Quaternion.Euler(0, 0, angle + 90) * Vector3.down,
                () => 9f);
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, angle + 180) * Vector3.down,
                () => 10f);
            SetBullet(bulletObjectPool2, firePoint.position,
                () => Quaternion.Euler(0, 0, angle + 270) * Vector3.down,
                () => 11f);

        }


    }


    private IEnumerator FireLineDelayed()
    {
        Vector3 center = transform.position;
        int bulletCount = 15;
        var target = GameObject.FindWithTag("Player");
        List<EnemyBullet> bullets = new List<EnemyBullet>();

        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            float rad = Mathf.Atan2(bulletDir.y, bulletDir.x);

            for (int i = 0; i < bulletCount; i++)
            {
                Vector3 offset = new Vector3(Mathf.Cos(rad) * i, Mathf.Sin(rad) * i, 0f);
                Vector3 spawnPos = center + 1.1f * offset;

                EnemyBullet bulletComp = SpawnBulletFromPool(
                    bulletObjectPool3,
                    spawnPos,
                    () => bulletDir,
                    () => 0f
                );

                if (bulletComp != null)
                    bullets.Add(bulletComp);

                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.2f);

        // 점점 빨라지는 속도 적용
        var newBulletDir = (target.transform.position - firePoint.position).normalized;

        foreach (var bulletComp in bullets)
        {
            if (bulletComp != null)
            {
                Vector3 individualDir = (target.transform.position - bulletComp.transform.position).normalized;
                bulletComp.SetDirection(() => individualDir);
                bulletComp.SetSpeed(() => 15f);
            }
        }
    }

    private EnemyBullet SpawnBulletFromPool(
    List<GameObject> bulletPool,
    Vector3 position,
    Func<Vector3> dirFunc,
    Func<float> speedFunc)
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            var bullet = bulletPool[i];

            if (bullet == null || bullet.activeSelf) continue;

            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);

            var bulletComponent = bullet.GetComponent<EnemyBullet>();
            bulletComponent.SetDirection(dirFunc);
            bulletComponent.SetSpeed(speedFunc);

            return bulletComponent;
        }

        Debug.LogWarning("[SpawnBulletFromPool] 사용 가능한 총알이 없습니다!");
        return null;
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
            // 여기서 Destroy 안 하고 애니메이션 실행
            animator.SetTrigger("Die");

            PlayerManager.Instance.ClearBulletPool();
            ClearBulletPool();
            // 스토리 연출 호출

            StartCoroutine(WaitAndShowStory(3f));
        }
    }

    IEnumerator WaitAndShowStory(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StoryUIManager.Instance.ShowStory(StoryType.Outro); // 싱글톤 or 다른 방식
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