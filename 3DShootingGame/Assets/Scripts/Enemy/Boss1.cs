using System.Collections;
using UnityEngine;

public class Boss1 : Enemy
{
    private int phase = 1;
    private Coroutine phaseRoutine;

    public GameObject bulletSpawnPointPrefab;
    private Transform[] bulletSpawnPoints;    // Transform �迭�� ����

    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        
        enemyScore = 10000;

        CreateBulletSpawnPoints(); // �۵� �ȵ�. ���� ���

        StartCoroutine(AfterStop(1f));
        StartPhase(phase); // ù ������ ����
    }

    void StartPhase(int newPhase)
    {
        // ���� ���� ����
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
                Debug.Log("���� ������ ����!");
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
            GameObject point = Instantiate(bulletSpawnPointPrefab, transform); // ���� ������ ����
            point.transform.localPosition = positions[i]; // ������ �������� �� ��� ��ġ
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
        // ���� �ֺ� ���� �߻� ����
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
                // 10���� ������ 3��
                float angle = -10f + (10f * i); // -10, 0, 10��
                Vector3 dir = Quaternion.Euler(0, 0, angle) * spawn.transform.up;

                GameObject bullet = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().SetDirection(dir);
                bullet.GetComponent<EnemyBullet>().SetSpeed(2.0f); // ������
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
        // ȥ�� ����: ���� ���� + ���� �ӵ�
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
                // 20���� ������ 3��
                float angle = -20f + (20f * i); // -20, 0, 20��
                Vector3 dir = Quaternion.Euler(0, 0, angle) * spawn.transform.up;

                GameObject bullet = Instantiate(bulletPrefab, spawn.transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().SetDirection(dir);
                bullet.GetComponent<EnemyBullet>().SetSpeed(4.0f); // ������
            }
        }
    }

    // === ������ ü�� ó�� ===
    protected new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HP--;

            if (HP <= 0)
            {
                if (phase < 3)
                {
                    StartPhase(phase + 1); // ���� �������
                }
                else
                {
                    var explosion = Instantiate(effect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    Debug.Log("���� óġ!");
                }

                ScoreManager.instance.Score += enemyScore;
            }

            other.gameObject.SetActive(false); // �Ѿ� ��Ȱ��ȭ
        }
    }

    IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }
}