using System.Collections;
using UnityEngine;

public class Boss1 : Enemy
{
    private int phase = 1;
    private Coroutine phaseRoutine;

    public GameObject bulletSpawnPointPrefab;
    private Transform[] bulletSpawnPoints;    // Transform 배열로 관리

    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        
        enemyScore = 10000;

        CreateBulletSpawnPoints(); // 작동 안됨. 추후 고려

        StartCoroutine(AfterStop(1f));
        StartPhase(phase); // 첫 페이즈 시작
    }

    void StartPhase(int newPhase)
    {
        // 이전 패턴 종료
        if (phaseRoutine != null) StopCoroutine(phaseRoutine);

        phase = newPhase;

        switch (phase)
        {
            case 1:
                HP = 100;
                phaseRoutine = StartCoroutine(Phase1Pattern());
                break;

            case 2:
                HP = 200;
                phaseRoutine = StartCoroutine(Phase2Pattern());
                break;

            case 3:
                HP = 250;
                phaseRoutine = StartCoroutine(Phase3Pattern());
                break;

            default:
                Debug.Log("보스 페이즈 종료!");
                break;
        }
    }

    // === Phase 1 ===
    IEnumerator Phase1Pattern()
    {
        while (true)
        {
            FireBullet1phase();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FireBullet1phase()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            for (int i = 5; i < 10; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector3 TanbulletDir = Quaternion.Euler(0, 0, j * 90) * bulletDir;
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    bullet.GetComponent<EnemyBullet>().SetDirection(TanbulletDir);
                    bullet.GetComponent<EnemyBullet>().SetSpeed(2.5f * i);
                }
            }
        }
    }

    // === Phase 2 ===

    private void CreateBulletSpawnPoints()
    {
        Vector3[] positions = new Vector3[]
        {
        new Vector3(-8,  1, 0),
        new Vector3(-8,  7, 0),
        new Vector3(-8, -5, 0),
        new Vector3( 8,  1, 0),
        new Vector3( 8,  7, 0),
        new Vector3( 8, -5, 0)
        };

        bulletSpawnPoints = new Transform[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject point = Instantiate(bulletSpawnPointPrefab, transform); // 보스 하위에 생성
            point.transform.localPosition = positions[i]; // 보스를 기준으로 한 상대 위치
            bulletSpawnPoints[i] = point.transform;
        }
    }


    IEnumerator Phase2Pattern()
    {
        while (true)
        {
            FireBulletPhase2();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void FireBulletPhase2()
    {
        // 보스 주변 원형 발사 예시
        int bulletCount = 18;
        float angle2 = Random.Range(0f, 360f);
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector3 dir = Quaternion.Euler(0, 0, angle + angle2) * Vector3.down;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetDirection(dir);
            bullet.GetComponent<EnemyBullet>().SetSpeed(6f);
        }

        foreach (var spawn in bulletSpawnPoints)
        {
            for (int i = 0; i < 3; i++)
            {
                // 10도씩 퍼지는 3발
                float angle = -10f + (10f * i); // -10, 0, 10도
                Vector3 dir = Quaternion.Euler(0, 0, angle) * spawn.transform.up;

                GameObject bullet = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().SetDirection(dir);
                bullet.GetComponent<EnemyBullet>().SetSpeed(2.0f); // 느리게
            }
        }
    }

    // === Phase 3 ===
    IEnumerator Phase3Pattern()
    {
        while (true)
        {
            FireBulletPhase3();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FireBulletPhase3()
    {
        // 혼합 패턴: 랜덤 각도 + 빠른 속도
        for (int i = 0; i < 15; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.down;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetDirection(dir);
            bullet.GetComponent<EnemyBullet>().SetSpeed(8f);
        }

        foreach (var spawn in bulletSpawnPoints)
        {
            for (int i = 0; i < 3; i++)
            {
                // 20도씩 퍼지는 3발
                float angle = -20f + (20f * i); // -20, 0, 20도
                Vector3 dir = Quaternion.Euler(0, 0, angle) * spawn.transform.up;

                GameObject bullet = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().SetDirection(dir);
                bullet.GetComponent<EnemyBullet>().SetSpeed(4.0f); // 느리게
            }
        }
    }

    // === 페이즈 체력 처리 ===
    protected new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HP--;

            if (HP <= 0)
            {
                if (phase < 3)
                {
                    StartPhase(phase + 1); // 다음 페이즈로
                }
                else
                {
                    var explosion = Instantiate(effect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    Debug.Log("보스 처치!");
                }

                ScoreManager.instance.Score += enemyScore;
            }

            other.gameObject.SetActive(false); // 총알 비활성화
        }
    }

    IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }
}