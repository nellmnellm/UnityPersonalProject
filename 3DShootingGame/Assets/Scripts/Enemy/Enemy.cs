using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("적 정보")]
    public float speed;
    public Vector3 dir;
    public int HP;
    public int enemyScore;

    public GameObject effect;


    
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

    
    //충돌 끝
    protected void OnCollisionExit(Collision collision)
    {
        
    }
    //충돌 진행 상황.
    protected void OnCollisionStay(Collision collision)
    {
        
    }

}
