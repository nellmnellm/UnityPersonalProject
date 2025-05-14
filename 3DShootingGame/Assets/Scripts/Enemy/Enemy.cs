using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("�� ����")]
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

    
    //�浹 ��
    protected void OnCollisionExit(Collision collision)
    {
        
    }
    //�浹 ���� ��Ȳ.
    protected void OnCollisionStay(Collision collision)
    {
        
    }

}
