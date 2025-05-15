using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{

  
    Animator animator;

    //����, ��ų, �뽬 �� �ð�
    float lastAttackTime, lastSkillTime, lastDashTime;
    public bool attacking = false;
    public bool dashing = false;

    // ui�� controller ��ġ.
    float h, v;
    /// <summary>
    /// UI ��ư ���� �̿��ؼ� ������ ����.
    /// </summary>
    /// <param name="stickPos"></param>
    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }
    //����Ƽ �ڷ�ƾ �ٽ� �������̽�. �Ͻ� �ߴ��� ������ �Լ��� �����ϴµ� ���Ǵ� Ÿ��
    //visualscripting
    IEnumerator Attack()
    {
        if (Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            while (attacking)
            {
                animator.SetTrigger("Attack");
                //trigger�� �����ϴ°����� ������ �ٷ� ����.
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
        //������ ����Ǿ� �۵��ϵ��� ó��.
        if (animator)
        {
            float back = 1f;
            if (v < 0f)
                back = -1;

            //animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Speed", new Vector2(h,v).magnitude);
            Rigidbody rbody = GetComponent<Rigidbody>();
            //����Ϸ�
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
