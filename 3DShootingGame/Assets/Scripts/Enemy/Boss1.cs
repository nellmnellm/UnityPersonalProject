using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss1 : Enemy
{
    private int phase = 1;
    private Coroutine phaseRoutine;
    [Header("2, 3phase �Ѿ�")]
    public GameObject bulletPrefab2; // �Ķ�. �������� ���� ����
    


    private Transform[] bulletSpawnPoints;    // Transform �迭�� ����

    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        
        enemyScore = 10000;

        StartCoroutine(AfterStop(1f));
        StartPhase(phase); // ù ������ ����
    }

    private void StartPhase(int newPhase)
    {
        // ���� ���� ����
        if (phaseRoutine != null) StopCoroutine(phaseRoutine);

        phase = newPhase;

        switch (phase)
        {
            case 1:
                HP = 150;
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
    private IEnumerator Phase1Pattern()
    {
        while (true)
        {
            FireBullet1phase();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FireBullet1phase()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            for (int i = 5; i < 10; i++)
            {
                int currentI = i; // C# Ŭ���� ����! �����Լ��� ������ �ϴٰ� �������� ȣ���ϴµ� 9�� �ݿ���. ���߿� �� �˾ƺ���.
                for (int j = 0; j < 6; j++)
                {
                    CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, j * 60) * bulletDir, () => 2.2f * currentI); 
                    //CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, j * 60) * bulletDir, () => 2.2f * i);
                }
            }
        }
    }

    // === Phase 2 ===

    private void CreateBulletSpawnPoints(float angle, float speed)
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
            for (int j = 0; j < 3; j++)
            {
                // n���� ������ 3��
                float angle3 = -angle + (angle * j); // -n, 0, n��
                
                CreateBullet(bulletPrefab, firePoint.position + positions[i], Quaternion.Euler(0,0,angle3) * Vector3.right, () => 4f);
            }
        }
       

    }


    private IEnumerator Phase2Pattern()
    {
        int count = 0;
        while (true)
        {
            if (count % 10 == 0)
            {
                CreateBulletSpawnPoints(20f, 2f);
            }

            FireBulletPhase2();
            count++;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void FireBulletPhase2()
    {
        // ���� �ֺ� ���� �߻� ����
        int bulletCount = 18;
        float angle2 = Random.Range(0f, 360f);
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector3 dir = Quaternion.Euler(0, 0, angle + angle2) * Vector3.down;

            CreateBullet(bulletPrefab2, firePoint.position, dir, () => 8);
        }
    }
    
    // === Phase 3 ===
    private IEnumerator Phase3Pattern()
    {
        int count = 0;
        while (true)
        {
            if (count % 10 == 0)
            {
                CreateBulletSpawnPoints(30f, 3f);
            }
            FireBulletPhase3();
            count++;

            yield return new WaitForSeconds(0.15f);
        }
    }

    private void FireBulletPhase3()
    {
        // ȥ�� ����: ���� ���� + ���� �ӵ�
        for (int i = 0; i < 15; i++)
        {
            float angle = Random.Range(0f, 360f);
            CreateBullet(bulletPrefab2, firePoint.position, Quaternion.Euler(0, 0, angle) * Vector3.down, () => 8f);
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

    private IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }
}