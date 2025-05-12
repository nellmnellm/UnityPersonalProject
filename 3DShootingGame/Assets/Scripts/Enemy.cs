using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
   
    public float speed;
    public GameObject effect;
    public Vector3 dir;
    public int HP;

    protected void Start()
    {
      /*  int randValue = Random.Range(0, 10);
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
        }*/
    }


    protected void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
   
    //충돌 시작
    protected void OnCollisionEnter(Collision collision)
    {
        HP--;
        if (HP == 0)
        {
            var explosion = Instantiate(effect);
            explosion.transform.position = transform.position;


            //Destroy(collision.gameObject);
            Destroy(gameObject);
            //부딛히면 죽음
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
