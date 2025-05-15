using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{

  
    Animator animator;

    //공격, 스킬, 대쉬 의 시간
    float lastAttackTime, lastSkillTime, lastDashTime;
    public bool attacking = false;
    public bool dashing = false;

    // ui의 controller 배치.
    float h, v;
    /// <summary>
    /// UI 버튼 등을 이용해서 공격을 진행.
    /// </summary>
    /// <param name="stickPos"></param>
    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }
    //유니티 코루틴 핵심 인터페이스. 일시 중단이 가능한 함수를 정의하는데 사용되는 타입
    //visualscripting
    IEnumerator Attack()
    {
        if (Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            while (attacking)
            {
                animator.SetTrigger("Attack");
                //trigger는 설정하는것으로 조건을 바로 만족.
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
    public void OnAttackDown()
    {
        attacking = true;
        animator.SetBool("Combo", true);
        StartCoroutine(Attack());
    }

    public void OnAttackUp() 
    {
        attacking = false;
    }  
    
    public void OnSkillDown()
    {
        if (Time.time - lastSkillTime > 1.0f)
        {
            animator.SetBool("Skill", true);
            lastSkillTime = Time.time;
        }
        
    }

    public void OnSkillUp()
    {
        animator.SetBool("Skill", false);
    }

    public void OnDashDown()
    {
        dashing = true;
    }

    public void OnDashUp()
    {
        dashing = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //연결이 진행되야 작동하도록 처리.
        if (animator)
        {
            float back = 1f;
            if (v < 0f)
                back = -1;

            //animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Speed", new Vector2(h,v).magnitude);
            Rigidbody rbody = GetComponent<Rigidbody>();
            //연결완료
            if (rbody)
            {
                Vector3 speed = rbody.linearVelocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rbody.linearVelocity = speed;

                if (h != 0f && v != 0f)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
                }
            }
        }
    }

}
