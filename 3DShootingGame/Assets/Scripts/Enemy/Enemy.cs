using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab; //�Ѿ� ������
    public Transform firePoint;     //�Ѿ� �����°� (���� ��ġ)
    public GameObject effect;       //�� �״� ȿ��
    [Header("������Ʈ Ǯ")]
    public int initialPoolSize = 100;    //** �Ѿ��� ����
    protected List<GameObject> bulletObjectPool = new List<GameObject>(); //** ������Ʈ Ǯ
    

    //[Header("�� ����")]
    protected float speed;          //�� �ӵ�
    protected Vector3 dir;          //���� ���� ( �����ʿ��� ��ȯ�Ǵ� ��찡 �����Ƿ� �����ϰ� �����Ұ�)
    protected int HP;               //���� HP
    protected int enemyScore;       //���� �ִ� ����


    /*   protected void Start()
      {
         int randValue = Random.Range(0, 10);
          if (randValue < 3)
          {
              //���� �ſ��� player�� �˻�
              var target = GameObject.Find("Player");
              dir = target.transform.position - transform.position;
              dir.Normalize();

          }

          else if (randValue < 8)
          {
              dir = Vector3.down;
          }

          else
          {
              dir = Vector3.down * 3;
          }
    }*/
    /// <summary>
    ///  �Ѿ� ������ƮǮ ���� 
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="bulletPool"></param>
    /// <returns></returns>
    protected virtual GameObject AddBulletToPool(GameObject bulletPrefab, List<GameObject>bulletPool)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletPool.Add(bullet);
        return bullet;
    }
   
    /// <summary>
    /// �⺻ �Ѿ� �߻� ����
    /// </summary>
    protected virtual void FireBullet()
    {

    }
    /// <summary>
    /// ���� �� �⺻ �Ѿ� �߻� ������ ���� ������ 
    /// </summary>
    /// <param name="delaySeconds"></param>
    /// <returns></returns>
    protected virtual IEnumerator bulletStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        CancelInvoke(nameof(FireBullet));

    }
    /// <summary>
    /// �Ѿ� �����Ҷ� �̿��� �޼��� 
    /// </summary>
    /// <param name="bulletObjectPool"></param>
    /// <param name="position"></param>
    /// <param name="directionFunction"></param>
    /// <param name="speedFunction"></param>
    protected void SetBullet(List<GameObject> bulletObjectPool, Vector3 position, Func<Vector3> directionFunction, Func<float> speedFunction)
    {
        bool bulletFound = false;
        for (int i = 0; i < initialPoolSize; i++) // ������Ʈ Ǯ
        {
            var bullet = bulletObjectPool[i];

            if (bullet == null || bullet.Equals(null)) continue;
            if (!bullet.activeSelf)
            {
                bullet.transform.position = position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);
                var bulletComponent = bullet.GetComponent<EnemyBullet>();
                bulletComponent.SetDirection(directionFunction);
                bulletComponent.SetSpeed(speedFunction);
                bulletFound = true;
                break;
            }
            //������ ������ƮǮ ������ �������. => ���� �ȳ���!!!!!���� ���Ͼ�� ���� �� ����
            /*GameObject newBullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            newBullet.SetActive(true);
            var newBulletComponent = newBullet.GetComponent<EnemyBullet>();
            newBulletComponent.SetDirection(directionFunction);
            newBulletComponent.SetSpeed(speedFunction); 
            bulletObjectPool.Add(newBullet); // Ǯ�� �߰�
            ���İ��� �����ӵ���� �ʹ� ���ؼ� ������. �Ѿ� ���� ���� ���������� ���� �̽�*/
            
        }
        if (!bulletFound)
        {
            Debug.LogWarning($"[SetBullet] ��� ������ �Ѿ��� �����ϴ�! Ǯ ũ��: {bulletObjectPool.Count}");
        }

    }

    /// <summary>
    /// �Ѿ� �������� �����Ҷ� (������Ʈ Ǯ�� �ƴ� �ٸ� �Ѿ�)
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="position"></param>
    /// <param name="directionFunction"></param>
    /// <param name="speedFunction"></param>
    protected void CreateBullet(GameObject bulletPrefab, Vector3 position, Func<Vector3> directionFunction, Func<float> speedFunction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        var bulletComponent = bullet.GetComponent<EnemyBullet>();
        bulletComponent.SetDirection(directionFunction);
        bulletComponent.SetSpeed(speedFunction);

    }

    //3 Stage ���� �̵��� �޼���
    protected IEnumerator MoveBossToPosition(Vector3 target, float duration)
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
    }

    protected virtual void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddBulletToPool(bulletPrefab, bulletObjectPool);
        }
    }
    protected void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
   
    //�浹 ����
    /*protected void OnCollisionEnter(Collision collision)
    {
        HP--;
        if (HP == 0)
        {
            var explosion = Instantiate(effect);
            explosion.transform.position = transform.position;


            //Destroy(collision.gameObject);
            Destroy(gameObject);
            ScoreManager.instance.Score += enemyScore;
            //�ε����� ����
        }
    }*/

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // Tag�� �Ѿ����� Ȯ��
        {
            HP--;
            if (HP == 0)
            {
                var explosion = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                ScoreManager.Instance.Score += enemyScore;
            }
            other.gameObject.SetActive(false); //** �Ѿ� ��Ȱ��ȭ(������ƮǮ)
        }
        else if (other.CompareTag("Bomb"))
        {
            HP -= 50;
            if (HP <= 0)
            {
                var explosion = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                ScoreManager.Instance.Score += enemyScore;
            }
        }
    }

    protected void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}
