using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1f;
    public int attackDamage = 10;

    //Animator animator;

    GameObject player;                    //�÷��̾� ����
    PlayerHealth playerHealth;                  //�÷��̾� ü��
    EnemyHealth enemyHealth;              //������ ü��
    bool playerInRange;                   //�÷��̾���� �Ÿ�
    float timer;                          //�ð� üũ

    private void Awake()
    {
        //FindAnyObjectbytype<player>() ; ���� ��쿡�� ������ Ȱ��ȭ�� player ã�� => ����Ƽ2023 ���� ��õ
        //Gameobject.Find("name") : �̸��� ������ �� ä���ϹǷ� Ÿ�Ծ�����,������ �ſ� ������
        //GameObject.FindGameObjectWithTag("tag��") => �ش� �±׸� ���� ������Ʈ. �迭�� 0�� �ϳ��� ã��
        //GameObject.FindWithTag("tag��") => �����
        //��ũ��Ʈ�� ���� ����
        //�Ŵ��� ���� 
        //player = FindAnyObjectByType<PlayerHealth>();
        player= GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Update()
    {
       
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
           
            Attack();
            
        }
    }

    private IEnumerator AttackDelay(float delaytime)
    {
        //animator.SetBool("Attack", true);
        yield return new WaitForSeconds(delaytime);
        //animator.SetBool("Attack", false);
    }
    private void Attack()
    {
        timer = 0;
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        //animator.SetBool("Attack", true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }
}


