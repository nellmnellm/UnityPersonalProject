using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;

    public float flashSpeed = 5.0f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f);
    public float sinkSpeed = 1.0f;

    bool isSinking, isDead;
    public bool damaged;
    private void Awake()
    {
        currentHealth = startingHealth;


    }

    private void Update()
    {
        //데미지 처리에 따라 슬라임의 색을 변경하는 코드
        if (damaged)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", flashColor);
            Debug.Log("체크");

        }
        else
        {
            transform.GetChild(0).GetComponent<Renderer>().
            material.SetColor("_Color",
            Color.Lerp(transform.GetChild(0).
            GetComponent<Renderer>().
            material.GetColor("_Color"), Color.white, flashSpeed * Time.deltaTime));

        }
        damaged = false;

        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth =- amount;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public IEnumerator StartDamage(int damage, Vector3 playerPosition, float delay, float pushBack)
    {
        yield return new WaitForSeconds(delay);

        try
        {
            TakeDamage(damage);
            
            //플레이어로부터 멀어질 방향.
            Vector3 diff = (playerPosition - transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(diff * 50f * pushBack, ForceMode.Impulse);
            //ForceMode.Impulse : 순간적인 힘 (점프 대시 밀림 등)을 구현
            //ForceMode.Force : 지속적인 힘을 구현 할 때 사용(오브젝트 밀기. 엔진 추진, 바람효과 등)

        }
        catch(MissingReferenceException e) //객체참조가 유효하지 않은 상황에 안내 (이미죽어가는상황?)
        {
            Debug.Log(e.ToString());
        }
    }
    private void Death()
    {
        StageController.Instance.AddPoint(10);
        isDead = true;
        transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
        StartSinking();

        DialogControllerQuest quest = FindObjectOfType<DialogControllerQuest>();
        if (quest != null)
        {
            quest.questSuccess = true;
        }
    }

    private void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2.0f);
    }
}
