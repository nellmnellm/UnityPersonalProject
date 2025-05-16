using System.Collections;
using UnityEngine;

//1.�ش� ��ũ��Ʈ�� ��� �� �ִϸ����� ������Ʈ�� �䱸�մϴ�.
// -> �ش� �Ӽ��� ���ԵǾ��ִ� ��ũ��Ʈ�� ������Ʈ�� �������� ���
//    ������ ���� ó���˴ϴ�.
//    1. �䱸�ϰ� �ִ� �ִϸ����� ������Ʈ�� ���� ��� �ڵ� �������ݴϴ�.
//    2. �� ��ũ��Ʈ�� ������Ʈ�� ���Ǵ� ����, � ����
//       �䱸�ϴ� ������Ʈ�� ������ �� �����ϴ�.
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    Animator animator;

    //���� , ��ų, ��ÿ� ���� �ð�
    float lastAttackTime, lastSkillTime, lastDashTime;
    public bool attacking = false;
    public bool dashing = false;

    //UI�� ��Ʈ�ѷ��� ��ġ�ؼ� �� ��Ʈ�ѷ��� �̵��� �����غ� ����
    float h, v;

    //��ƽ�� ��ġ�� ���޹޾Ƽ� x�� y���� ó���մϴ�.
    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }
    //UI�� ��ư ���� �̿��ؼ� ������ �����ؾ� �ϹǷ�, ��� ���� �Լ� ������ �����ϴ� ������ ���� ����
    //XXXDown : ������ �� (1��)
    //XXXUp : ������ ���� �� (1��)
    //XXX : ������ �ִ� ����

    //OnAttackUp, OnSkillUp, OnDashUp ���� ������ �����ϱ� ���� ������ ���ǿ� ���� ó���ϴ� �Լ� ==> �÷��� �Լ�

    //��Ÿ ���ݿ� ���� �ڷ�ƾ ����
    private IEnumerator Attack()
    {
        if(Time.time - lastAttackTime > 0.5f)
        {
            lastAttackTime = Time.time;
            while(attacking)
            {
                animator.SetTrigger("Attack");
                //�ִϸ������� �Ķ���� �߿��� SetTrigger��
                //�����ϴ� ������ ������ �ٷ� �����ϰ� �˴ϴ�.
                //���� ������ ��
                yield return new WaitForSeconds(0.5f);
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
        animator.SetBool("Combo", false);
    }
    public void OnSkillDown()
    {
        if(Time.time - lastSkillTime > 0.5f)
        {
            animator.SetBool("Skill", true);
            lastSkillTime = Time.time;
        }
    }
    public void OnSkillUp()
    {
        animator.SetBool("Skill", false);
    }

    IEnumerator DashHolding()
    {
        dashing = true;
        yield return new WaitForSeconds(0.7f);
        OnDashToggle();
    }

    public void OnDashToggle()
    {

        if (dashing == false)
        {
            animator.SetBool("Dash", true);
            StartCoroutine(DashHolding());
        }
        else
        {
            animator.SetBool("Dash", false);
            dashing = false;
        }
    }
   /* public void OnDashUp()
    {
        dashing = false;
    }*/
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ִϸ����Ϳ� ���� ������ ����Ǿ� �۵��ϵ��� ó���մϴ�.
        if(animator)
        {
            //�̵� ����(����)
            float back = 1f;
            if (v < 0f)
                back = -1f;

            animator.SetFloat("Speed", new Vector2(h,v).magnitude);
            //magnitude == ������ ����,ũ��

            Rigidbody rbody = GetComponent<Rigidbody>();

            //������ٵ� ����Ǿ����� ��
            if(rbody)
            {
                Vector3 speed = rbody.linearVelocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rbody.linearVelocity = speed;

                //���� ��ȯ
                if(h != 0f &&  v != 0f)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
                }
            }
        }
    }
}