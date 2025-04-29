using UnityEngine;

public class Unit : MonoBehaviour
{
    Animator animator;
    [Header("�÷��̾� �ɷ�ġ")]
    //��ġ���� �ɷ�ġ ������(������ �뷮��) ũ�� �����
    public double HP;
    public double ATK;
    public double ATK_SPEED;

    [Header("���� ����")]
    public float A_RANGE; //��Ÿ�
    public float T_RANGE; //�߰� ����

    [Header("Ÿ�� ��ġ")]
    public Transform target;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        //�ڵ� ������ animator�� �ν��ϰ� �ʵ峪 �޼��� ��밡��.
    }

    protected void SetAnimator(string temp)
    {
        // boolŸ���� �ƴ� Trigger Ÿ���� �̷��� ó��
        if (temp == "IsATTACK")
        {
            animator.SetTrigger("IsATTACK");
            return;
        }
        //�⺻ �Ķ���Ϳ� ���� reset
        animator.SetBool("IsIDLE", false);
        animator.SetBool("IsMOVE", false);
        //���ڷ� ���޹��� ���� true��
        animator.SetBool(temp, true);
    }

    protected void StrikeFirst<T>(T[] targets) where T : Component
    {
        var enemys = targets;
        Transform closet = null;
        var max = T_RANGE;

        for (int i=0; i<enemys.Length; i++)
        {
            var target_distance = Vector3.Distance(
                transform.position, enemys[i].transform.position);
            if (target_distance < max)
            {
                //���� ����� �Ÿ�
                closet = enemys[i].transform;
                //�ִ� �Ÿ��� Ÿ���� �Ÿ��� ����.
                max = target_distance;
            }
            

        }
        //���� ����� ���� Ÿ������.
        target = closet;
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
  
}
