using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerManager : MonoBehaviour
{
    public int HP = 5;                     // 플레이어의 목숨 수
    public int level = 1;                  // 총알 개수
    public GameObject bulletFactory;       // bullet prefab 할당
    //public int score = 0;

    [Header("피격시 무적 조절")]
    public GameObject effect;              // 피격시 이펙트
    public float invincibleDuration = 2f;  // 피격시 무적 시간
    private bool isInvincible = false;     // 반짝이는 효과 적용 여부
    private Renderer[] renderers;          // 반짝이는 효과 적용을 위한 renderer 집합(구조의 하위구조)
    

    public GameObject[] firePosition;      //

    [Header("오브젝트 풀")]
    public int initialPoolSize = 100;    //** 총알의 개수
    public List<GameObject> bulletObjectPool = new List<GameObject>(); //** 오브젝트 풀


    private GameObject AddBulletToPool()
    {
        var bullet = Instantiate(bulletFactory);
        bullet.SetActive(false);
        bulletObjectPool.Add(bullet);
        return bullet;
    }

    void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddBulletToPool();
        }
        // 모든 자식의 Renderer (몸이 여러 MeshRenderer로 이루어져 있을 수 있음)
        renderers = GetComponentsInChildren<Renderer>();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            for (int j=0; j<level; j++)
            {
                for (int i = 0; i < initialPoolSize; i++)              //** 오브젝트풀
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
        
       /* if (Input.GetButtonDown("Fire1"))
        {
            
            for (int i=0; i<level; i++)
            {
                var bullet = Instantiate(bulletFactory);
                bullet.transform.position = firePosition[i].transform.position;

            }
           
        }*/
    }


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