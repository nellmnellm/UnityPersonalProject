using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab; //총알 프리펩
    public Transform firePoint;     //총알 나오는곳 (적의 위치)
    public GameObject effect;       //적 죽는 효과

    //[Header("적 정보")]
    protected float speed;          //적 속도
    protected Vector3 dir;          //적의 방향 ( 스포너에서 소환되는 경우가 많으므로 감안하고 설정할것)
    protected int HP;               //적의 HP
    protected int enemyScore;       //적이 주는 점수


    /*   protected void Start()
      {
         int randValue = Random.Range(0, 10);
          if (randValue < 3)
          {
              //게임 신에서 player를 검색
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
   
    //충돌 시작
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
            //부딛히면 죽음
        }
    }*/

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // Tag로 총알인지 확인
        {
            HP--;
            if (HP == 0)
            {
                var explosion = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                ScoreManager.instance.Score += enemyScore;
            }
            // Destroy(other.gameObject); // 총알 파괴
            other.gameObject.SetActive(false); //** 총알 비활성화(오브젝트풀)
        }
    }
}
