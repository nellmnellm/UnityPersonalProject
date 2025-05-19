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
    [Header("�ٸ� ������ �Ѿ�")]
    public GameObject bulletPrefab2; // �Ķ�. �������� ���� ����
    public int initialPoolSize2 = 800;    //** �Ѿ��� ����
    protected List<GameObject> bulletObjectPool2 = new List<GameObject>(); //** ������Ʈ Ǯ

    private Animator animator;
    private bool isDead = false;

    //hp UI ����
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
                Debug.Log("���� ������ ����!");
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
                    int currentI = i; // C# Ŭ���� ����! �����Լ��� ������ �ϴٰ� �������� ȣ���ϴµ� 9�� �ݿ���. ���߿� �� �˾ƺ���.
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
        Vector3 center = transform.position; // ������ ��ġ
        int bulletCount = 16;

        List<GameObject> bullets = new List<GameObject>();

        //  ��迡 �Ѿ� ���� (����)
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad) * 4f, Mathf.Sin(rad) * 4f, 0f); // ������ 15

            Vector3 spawnPos = center + offset;
            Vector3 targetDir = (center - spawnPos).normalized;

            GameObject bullet = Instantiate(bulletPrefab2, spawnPos, Quaternion.identity);
            var bulletComp = bullet.GetComponent<EnemyBullet>();

            bulletComp.SetDirection(() => targetDir);
            bulletComp.SetSpeed(() => 0f); // �ʱ�ӵ� 0

            bullets.Add(bullet);
        }

        // 1�� ���
        yield return new WaitForSeconds(1f);

        // ���� �������� �ӵ� ����
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
                // n���� ������ 3��
                float angle3 = -angle + (angle * j); // -n, 0, n��
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
        // ���� �ֺ� ���� �߻� ����
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
        // ȥ�� ����: ���� ���� + ���� �ӵ�
        for (int i = 0; i < 12; i++)
        {
            float angle = Random.Range(0f, 360f);
            SetBullet(bulletObjectPool2, firePoint.position, () => Quaternion.Euler(0, 0, angle) * Vector3.down, () => 8f);
            //CreateBullet(bulletPrefab2, firePoint.position, () => Quaternion.Euler(0, 0, angle) * Vector3.down, () => 8f);
        }

        
    }

    // === ������ ü�� ó�� ===
    protected new void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // �̹� �׾����� ó�� �� ��

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
            isDead = false; // ���� ������ ���� �� �ٽ� ���� �� ����
        }
        else
        {
            ScoreManager.instance.Score += enemyScore;
            var explosion = Instantiate(effect, transform.position, Quaternion.identity);
            // ���⼭ Destroy �� �ϰ� �ִϸ��̼� ����
            animator.SetTrigger("Die");

            // ���丮 ���� ȣ��
            StartCoroutine(WaitAndShowStory(3f));
        }
    }

    IEnumerator WaitAndShowStory(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StoryUIManager.Instance.ShowStory(); // �̱��� or �ٸ� ���
    }

   
    
    
    private IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }

    
}