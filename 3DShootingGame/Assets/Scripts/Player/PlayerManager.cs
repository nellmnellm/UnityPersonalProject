using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    #region singleton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCam = Camera.main;
        playerAnimator = GetComponent<Animator>();
    }

    #endregion

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
    // public float invincibleDuration = 2f;  // �ǰݽ� ���� �ð�
    // => ���� IEnumerator�� ��Ȱ���ؼ� 5�� ���� �ο��ϱ� ���� �ڷ�ƾ ���� �Ķ���ͷ� ����. 
    
    private bool isInvincible = false;     // ��¦�̴� ȿ�� ���� ����
    private Renderer[] renderers;          // ��¦�̴� ȿ�� ������ ���� renderer ����(������ ��������)
                  // -> �����ð� ���� ����. �ν����Ϳ��� ����

    public GameObject[] firePosition;      //

    [Header("������Ʈ Ǯ")]
    public int initialPoolSize = 100;    //** �Ѿ��� ����
    public List<GameObject> bulletObjectPool = new List<GameObject>(); //** ������Ʈ Ǯ

    
    public GameObject AddBulletToPool()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletObjectPool.Add(bullet);
        return bullet;
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
        bulletObjectPool.Clear();
    }

    public void UpdateBombs()
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
    public void UpdateHearts()
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
            if (HP >= 6)
            {
                return;
            }
            else
            {
                // ���ο� ��Ʈ ����
                for (int i = 0; i < HP; i++)
                {
                    GameObject heart = Instantiate(heartImagePrefab, heartContainer);
                    hearts.Add(heart);
                }
            }
                
                
        }
    }
    //�� �̵��� ���
    public void SetUIContainers(Transform newHeartContainer, Transform newBombContainer)
    {
        heartContainer = newHeartContainer;
        bombContainer = newBombContainer;
        UpdateHearts();
        UpdateBombs();
    }

    private void Start()
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

        if (Input.GetButton("Fire1") && fireCooldownTimer <= 0f && bulletObjectPool.Count > 0
            && GetComponent<Collider>().enabled == true)
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
        var collider = GetComponent<Collider>();
        // �ƾ� ī�޶� Ȱ��ȭ
        mainCam.enabled = false;
        cutsceneCam.enabled = true;
        collider.enabled = false;
        //transform.position = new Vector3(0, -50, 0);
        // �ִϸ��̼� Ʈ����
        playerAnimator.SetTrigger("Punch");

        // �ƾ� ����
        yield return new WaitForSecondsRealtime(cutsceneTime);

        // ī�޶� ����
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        collider.enabled = true;
        //transform.position = new Vector3(0, -5, 0);
        StartCoroutine(InvincibilityCoroutine(2f));
        // ��ź ȿ�� ����
        GameObject bomb = Instantiate(bombPrefab, new Vector3(0, -1f, -0.1f), Quaternion.identity);
        bomb.SetActive(true);
        
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
        bombCount = 0;
        UpdateHearts();
        var explosion = Instantiate(effect);
        explosion.transform.position = transform.position;
        if (HP <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnAfterDelay(1f));
        }


    }

    void GameOver()
    {
        ScoreManager.Instance.ShowGameOverPanel();
    }
   

    IEnumerator RespawnAfterDelay(float delay)
    {
        var collider = GetComponent<Collider>();
        // ������ & �浹 ���� �� ���̰� ��
        collider.enabled = false;
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = false;
        }
        //transform.position = new Vector3(0f, -50f, 0f);
        yield return new WaitForSeconds(delay);
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = true;
        }
        bombCount = 2;
        UpdateBombs();
        collider.enabled = true;
        transform.position = new Vector3(0f, -5f, 0f);
        StartCoroutine(InvincibilityCoroutine(4f));
    }

    IEnumerator InvincibilityCoroutine(float invincibleDuration)
    {
        

        float blinkInterval = 0.2f;
        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
            isInvincible = true;
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