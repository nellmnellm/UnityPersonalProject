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

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        //�ڵ� ������ animator�� �ν��ϰ� �ʵ峪 �޼��� ��밡��.
    }

    protected void SetAnimator(string temp)
    {
        //�⺻ �Ķ���Ϳ� ���� reset
        animator.SetBool("IsIDLE", false);
        animator.SetBool("IsMOVE", false);
        //���ڷ� ���޹��� ���� true��
        animator.SetBool(temp, true);
    }

  
}
