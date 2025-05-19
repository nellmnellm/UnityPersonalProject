using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1f;
    public int attackDamage = 10;

    //Animator animator;

    GameObject player;                    //플레이어 추적
    PlayerHealth playerHealth;                  //플레이어 체력
    EnemyHealth enemyHealth;              //슬라임 체력
    bool playerInRange;                   //플레이어와의 거리
    float timer;                          //시간 체크

    private void Awake()
    {
        //FindAnyObjectbytype<player>() ; 같은 경우에는 씬에서 활성화된 player 찾음 => 유니티2023 문서 추천
        //Gameobject.Find("name") : 이름이 같으면 다 채용하므로 타입안정성,성능이 매우 안좋음
        //GameObject.FindGameObjectWithTag("tag명") => 해당 태그를 가진 오브젝트. 배열의 0번 하나만 찾음
        //GameObject.FindWithTag("tag명") => 비슷함
        //스크립트에 직접 연결
        //매니저 관리 
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


