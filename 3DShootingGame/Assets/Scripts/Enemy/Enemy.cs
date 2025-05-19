using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    protected virtual GameObject AddBulletToPool(GameObject bulletPrefab, List<GameObject>bulletPool)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletPool.Add(bullet);
        return bullet;
    }

    protected void SetBullet(List<GameObject> bulletObjectPool, Vector3 position, Func<Vector3> directionFunction, Func<float> speedFunction)
    {
        bool bulletFound = false;
        for (int i = 0; i < initialPoolSize; i++) // ������Ʈ Ǯ
        {
            var bullet = bulletObjectPool[i];

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


    protected void CreateBullet(GameObject bulletPrefab, Vector3 position, Func<Vector3> directionFunction, Func<float> speedFunction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        var bulletComponent = bullet.GetComponent<EnemyBullet>();
        bulletComponent.SetDirection(directionFunction);
        bulletComponent.SetSpeed(speedFunction);

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
}
