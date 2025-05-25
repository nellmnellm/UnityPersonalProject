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

public class Boss4 : Enemy, IBoss
{
    private int phase = 1;
    private Coroutine phaseRoutine;
    [Header("�ٸ� ������ �Ѿ�")]
    public GameObject bulletPrefab2; // �Ķ�. �������� ���� ����
    public GameObject bulletPrefab3;
    public int initialPoolSize2 = 800;    //** �Ѿ��� ����
    public int initialPoolSize3 = 800;
    protected List<GameObject> bulletObjectPool2 = new List<GameObject>(); //** ������Ʈ Ǯ
    protected List<GameObject> bulletObjectPool3 = new List<GameObject>();
    private Animator animator;
    private bool isDead = false;
    public GameObject ClearText;


    //hp UI ����
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

        enemyScore = 50000;

        var UI = GameObject.Find("BossHP");
        if (UI != null)
        {
            var barUI = UI.GetComponent<BossHPBar>();
            barUI.SetBoss(this);
        }
        
        StartPhase(phase); // ���⼭ �����ϰ� ������ ����
        StartCoroutine(AfterStop(1f));
    }
    private void StartPhase(int newPhase)
    {
        // ���� ���� ����
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
                HP = 220;
                maxHP = 220;
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
                Debug.Log("���� ������ ����!");
                break;
        }
    }

    // === Phase 1 ===
    private IEnumerator Phase1Pattern()
    {
        yield return new WaitForSeconds(1f);
        float startTime = Time.time;
        while (true)
        {
            StartCoroutine(FireCircleDelayed(startTime));
            StartCoroutine(FireCircleDelayed2(startTime));

            yield return new WaitForSeconds(0.5f);
            startTime = Time.time;
        }

    }


    private IEnumerator FireCircleDelayed(float startTime)
    {


        //  ��迡 �Ѿ� ���� (����)
        var target = GameObject.FindWithTag("Player");

        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            float randomAngle = Random.Range(0, 9);
            for (int i = 0; i < 40; i++)
            {
                int currentI = i;

                // direction, speed�� ĸ���ؼ� �ֱ�
                SetBullet(bulletObjectPool, firePoint.position,
                    () => Quaternion.Euler(0, 0, (Time.time - startTime < 3)
                    ? (12f * currentI) + 10 * (Time.time - startTime) * (Time.time - startTime)
                    : 12f * currentI + randomAngle) * bulletDir,
                    () => (Time.time - startTime < 2.5) ? -10f * (Time.time - startTime) + 5f * (Time.time - startTime) * (Time.time - startTime) : 6.25f);
                yield return null;
            }
        }

    }
    private IEnumerator FireCircleDelayed2(float startTime)
    {
        //  ��迡 �Ѿ� ���� (����)
        var target = GameObject.FindWithTag("Player");

        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            float randomAngle = Random.Range(0, 9);
            for (int i = 0; i < 40; i++)
            {
                int currentI = i;

                // direction, speed�� ĸ���ؼ� �ֱ�
                SetBullet(bulletObjectPool2, firePoint.position,
                    () => Quaternion.Euler(0, 0, (Time.time - startTime < 3)
                    ? -(12f * currentI) - 10 * (Time.time - startTime) * (Time.time - startTime)
                    : -12f * currentI - randomAngle) * bulletDir,
                    () => (Time.time - startTime < 2.5) ? -10f * (Time.time - startTime) + 5f * (Time.time - startTime) * (Time.time - startTime) : 6.25f);
                yield return null;
            }
        }

    }




    // === Phase 2 ===

    private IEnumerator Phase2Pattern()
    {
        
        yield return new WaitForSeconds(3.5f);
        while (true)
        {
            // 1. ���� ��ġ�� �̵�
            Vector3 randomPos = new Vector3(Random.Range(-7f, 7f), Random.Range(3f, 7f), 0);
            yield return StartCoroutine(MoveBossToPosition(randomPos, 1f));

            // 2. ���� ����
            yield return StartCoroutine(FireFanBullets());
            yield return new WaitForSeconds(1.1f);
        }
    }

    private IEnumerator FireFanBullets()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 PlayerDir = (target.transform.position - firePoint.position).normalized;
            Vector3 perp = new Vector3(-PlayerDir.y, PlayerDir.x, 0); // ���� ����

            // 5���� ���� ����Ʈ ��� (-2 ~ +2 ������)
            for (int i = -2; i <= 2; i++)
            {
                Vector3 spawnPoint = transform.position + perp * i;

                // �ӵ� 3~10 ������ �Ѿ� 8�� ����
                for (int j = 0; j < 8; j++)
                {
                    Vector3 bulletDir = (transform.position + 2f * PlayerDir - spawnPoint).normalized;
                    float speed = Mathf.Lerp(3f, 17f, 2 * j / 14f); // ���� ����
                    SetBullet(bulletObjectPool, spawnPoint, () => bulletDir, () => speed);
                    SetBullet(bulletObjectPool2, spawnPoint, () => Quaternion.Euler(0, 0, 22) * bulletDir, () => speed);
                    SetBullet(bulletObjectPool3, spawnPoint, () => Quaternion.Euler(0, 0, -22) * bulletDir, () => speed);
                }
            }

            yield return null;
        }
    }


    // === Phase 3 ===
    private IEnumerator Phase3Pattern()
    {
        yield return StartCoroutine(MoveBossToPosition(new Vector3(0, 4, 0), 1f));
        yield return new WaitForSeconds(1.5f);
        while (true)
        {
            Vector3 randomPos = new Vector3(Random.Range(-7f, 7f), Random.Range(3f, 7f), 0);
            yield return StartCoroutine(MoveBossToPosition(randomPos, 0.5f));


            yield return StartCoroutine(FireDrillDelayed());
            yield return new WaitForSeconds(0.5f);
        }

    }

    

    private IEnumerator FireDrillDelayed()
    {
        float radius = 3f;


        for (int i = 0; i < 90; i++)
        {
            Vector3 spawnPoint = transform.position + radius * (Quaternion.Euler(0, 0, 2f * i) * Vector3.left);
            float RandomAngle = Random.Range(0, 360);
            float startTime = Time.time;
            SetBullet(bulletObjectPool, spawnPoint, () => Quaternion.Euler(0, 0, RandomAngle) * Vector3.up, () => 2 * (Time.time - startTime));

            yield return new WaitForSeconds(0.002f);
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 90; i++)
        {
            Vector3 spawnPoint = transform.position + radius * (Quaternion.Euler(0, 0, 2f * i) * Vector3.left);
            float RandomAngle = Random.Range(0, 360);
            float startTime = Time.time;
            SetBullet(bulletObjectPool2, spawnPoint, () => Quaternion.Euler(0, 0, RandomAngle) * Vector3.up, () => 2 * (Time.time - startTime));

            yield return new WaitForSeconds(0.002f);
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 90; i++)
        {
            Vector3 spawnPoint = transform.position + radius * (Quaternion.Euler(0, 0, 2f * i) * Vector3.left);
            float RandomAngle = Random.Range(0, 360);
            float startTime = Time.time;
            SetBullet(bulletObjectPool3, spawnPoint, () => Quaternion.Euler(0, 0, RandomAngle) * Vector3.up, () => 2 * (Time.time - startTime));

            yield return new WaitForSeconds(0.002f);
        }
    
        yield return new WaitForSeconds(0.5f);
        
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
            ScoreManager.Instance.Score += enemyScore;
            var explosion = Instantiate(effect, transform.position, Quaternion.identity);
            // ���⼭ Destroy �� �ϰ� �ִϸ��̼� ����
            animator.SetTrigger("Die");

            PlayerManager.Instance.ClearBulletPool();
            ClearBulletPool();
            // ���丮 ���� ȣ��
            MoveBossToPositionAndClearText(new Vector3(0, 4, 0), 1f);
            StartCoroutine(WaitAndShowStory(3f));
        }
    }
    IEnumerator MoveBossToPositionAndClearText(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        ClearText.SetActive(true);
    }

    IEnumerator WaitAndShowStory(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StoryUIManager.Instance.ShowStory(StoryType.Outro); // �̱��� or �ٸ� ���
    }

    public void ClearBulletPool()
    {
        foreach (var bullet in bulletObjectPool)
        {
            if (bullet != null)
            {
                Destroy(bullet); // �� �޸𸮿��� ����
            }
        }
        foreach (var bullet in bulletObjectPool2)
        {
            if (bullet != null)
            {
                Destroy(bullet); // �� �޸𸮿��� ����
            }
        }
        foreach (var bullet in bulletObjectPool3)
        {
            if (bullet != null)
            {
                Destroy(bullet); // �� �޸𸮿��� ����
            }

        }

    }


    private IEnumerator AfterStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }


}