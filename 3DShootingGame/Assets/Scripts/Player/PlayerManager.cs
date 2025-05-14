using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerManager : MonoBehaviour
{
    public int HP = 5;                     // �÷��̾��� ��� ��
    public int level = 1;                  // �Ѿ� ����
    public GameObject bulletFactory;       // bullet prefab �Ҵ�
    //public int score = 0;

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
        // ��� �ڽ��� Renderer (���� ���� MeshRenderer�� �̷���� ���� �� ����)
        renderers = GetComponentsInChildren<Renderer>();
    }


    private void Update()
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