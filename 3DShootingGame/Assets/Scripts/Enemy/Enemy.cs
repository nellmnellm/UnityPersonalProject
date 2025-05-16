using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab; //�Ѿ� ������
    public Transform firePoint;     //�Ѿ� �����°� (���� ��ġ)
    public GameObject effect;       //�� �״� ȿ��

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

    protected void CreateBullet(GameObject bulletPrefab, Vector3 position, Vector3 rotation, Func<float> speedFunction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        var bulletComponent = bullet.GetComponent<EnemyBullet>();
        bulletComponent.SetDirection(rotation);
        bulletComponent.SetSpeed(speedFunction);
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
                ScoreManager.instance.Score += enemyScore;
            }
            // Destroy(other.gameObject); // �Ѿ� �ı�
            other.gameObject.SetActive(false); //** �Ѿ� ��Ȱ��ȭ(������ƮǮ)
        }
    }
}
