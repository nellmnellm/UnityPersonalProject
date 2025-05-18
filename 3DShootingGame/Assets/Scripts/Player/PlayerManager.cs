using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("�Ѿ� ����, �߻� ����")]
    [SerializeField] private float fireInterval = 0.14f; //�߻� ����
    private float fireCooldownTimer = 0f;
    public GameObject bulletPrefab;       // bullet prefab �Ҵ�

    [Header("��� ����")]
    public int HP = 5;                    // �÷��̾��� ��� ��
    public GameObject heartImagePrefab;             // ��� �̹��� ������
    public List<GameObject> hearts = new List<GameObject>(5);
    public Transform heartContainer;      // ��� UI�� �� ��ġ

    [Header("���� ����")]
    public int level = 1;                  // �Ѿ� ���� (���� ���� �ȵ� �κ� => 3 ����)

    [Header("��ź ����")]
    
    public int bombCount = 2;
    public GameObject bombImagePrefab;
    public List<GameObject> bombs = new List<GameObject>(2);
    public Transform bombContainer;
    
    public GameObject bombPrefab;
    public float effectDuration = 2.0f;
    public float bombCooltime = 5f;
    private float lastBombTime = -999f;

    [Header("��ź �ƾ� ����")]
    public Camera mainCam;
    public Camera cutsceneCam;
    private Animator playerAnimator;
    public float cutsceneTime = 1.0f;

    [Header("�ǰݽ� ���� ����")]
    public GameObject effect;              // �ǰݽ� ����Ʈ
    public float invincibleDuration = 2f;  // �ǰݽ� ���� �ð�
    private bool isInvincible = false;     // ��¦�̴� ȿ�� ���� ����
    private Renderer[] renderers;          // ��¦�̴� ȿ�� ������ ���� renderer ����(������ ��������)
    

    public GameObject[] firePosition;      //

    [Header("������Ʈ Ǯ")]
    public int initialPoolSize = 100;    //** �Ѿ��� ����
    public List<GameObject> bulletObjectPool = new List<GameObject>(); //** ������Ʈ Ǯ


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
            // ���� ��Ʈ ����
            foreach (var bomb in bombs)
            {
                Destroy(bomb);
            }
            bombs.Clear();

            // ���ο� ��Ʈ ����
            for (int i = 0; i < bombCount; i++)
            {
                GameObject bomb = Instantiate(bombImagePrefab, bombContainer);
                bombs.Add(bomb);
            }
        }
    }
    void UpdateHearts()
    {
        
        // ��Ʈ ���� �ٸ��� �ٽ� ����
        if (hearts.Count != HP)
        {
            // ���� ��Ʈ ����
            foreach (var heart in hearts)
            {
                Destroy(heart);
            }
            hearts.Clear();

            // ���ο� ��Ʈ ����
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
        // ��� �ڽ��� Renderer (���� ���� MeshRenderer�� �̷���� ���� �� ����)
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        fireCooldownTimer -= Time.deltaTime;

        if (Input.GetButton("Fire1") && fireCooldownTimer <= 0f)
        {
            for (int j = 0; j < level; j++)
            {
                for (int i = 0; i < initialPoolSize; i++) // ������Ʈ Ǯ
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

            fireCooldownTimer = fireInterval; // ��Ÿ�� �ʱ�ȭ
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
        // �ƾ� ī�޶� Ȱ��ȭ
        mainCam.enabled = false;
        cutsceneCam.enabled = true;

        transform.position += new Vector3(0, 0, 2);
        // �ִϸ��̼� Ʈ����
        playerAnimator.SetTrigger("Punch");

        // �ƾ� ����
        yield return new WaitForSecondsRealtime(cutsceneTime);

        // ī�޶� ����
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        StartCoroutine(InvincibilityCoroutine());
        // ��ź ȿ�� ����
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
                 for (int i = 0; i < initialPoolSize; i++)              //** ������ƮǮ
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