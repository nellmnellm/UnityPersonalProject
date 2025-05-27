using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab; //총알 프리펩
    public Transform firePoint;     //총알 나오는곳 (적의 위치)
    public GameObject effect;       //적 죽는 효과
    [Header("오브젝트 풀")]
    public int initialPoolSize = 100;    //** 총알의 개수
    protected List<GameObject> bulletObjectPool = new List<GameObject>(); //** 오브젝트 풀
    

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
    /// <summary>
    ///  총알 오브젝트풀 설정 
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
    /// 기본 총알 발사 로직
    /// </summary>
    protected virtual void FireBullet()
    {

    }
    /// <summary>
    /// 몇초 후 기본 총알 발사 로직을 멈출 것인지 
    /// </summary>
    /// <param name="delaySeconds"></param>
    /// <returns></returns>
    protected virtual IEnumerator bulletStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        CancelInvoke(nameof(FireBullet));

    }
    /// <summary>
    /// 총알 설정할때 이용할 메서드 
    /// </summary>
    /// <param name="bulletObjectPool"></param>
    /// <param name="position"></param>
    /// <param name="directionFunction"></param>
    /// <param name="speedFunction"></param>
    protected void SetBullet(List<GameObject> bulletObjectPool, Vector3 position, Func<Vector3> directionFunction, Func<float> speedFunction)
    {
        bool bulletFound = false;
        for (int i = 0; i < initialPoolSize; i++) // 오브젝트 풀
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
            //정해진 오브젝트풀 갯수를 넘을경우. => 성능 안나옴!!!!!절대 안일어나게 개수 잘 설정
            /*GameObject newBullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            newBullet.SetActive(true);
            var newBulletComponent = newBullet.GetComponent<EnemyBullet>();
            newBulletComponent.SetDirection(directionFunction);
            newBulletComponent.SetSpeed(speedFunction); 
            bulletObjectPool.Add(newBullet); // 풀에 추가
            순식간에 프레임드랍이 너무 심해서 빼버림. 총알 수는 증가 가능하지만 성능 이슈*/
            
        }
        if (!bulletFound)
        {
            Debug.LogWarning($"[SetBullet] 사용 가능한 총알이 없습니다! 풀 크기: {bulletObjectPool.Count}");
        }

    }

    /// <summary>
    /// 총알 개개인을 구현할때 (오브젝트 풀이 아닌 다른 총알)
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

    //3 Stage 보스 이동용 메서드
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
                ScoreManager.Instance.Score += enemyScore;
            }
            other.gameObject.SetActive(false); //** 총알 비활성화(오브젝트풀)
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
