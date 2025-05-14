using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab;
    public Transform firePoint; 
    public GameObject effect;

    //[Header("�� ����")]
    protected float speed;
    protected Vector3 dir;
    protected int HP;
    protected int enemyScore;

    


    
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
