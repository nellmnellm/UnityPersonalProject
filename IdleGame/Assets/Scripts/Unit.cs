using UnityEngine;

public class Unit : MonoBehaviour
{
    Animator animator;
    [Header("플레이어 능력치")]
    //방치형은 능력치 범위를(데이터 용량을) 크게 잡는편
    public double HP;
    public double ATK;
    public double ATK_SPEED;

    [Header("공격 범위")]
    public float A_RANGE; //사거리
    public float T_RANGE; //추격 범위

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        //코드 내에서 animator를 인식하고 필드나 메서드 사용가능.
    }

    protected void SetAnimator(string temp)
    {
        //기본 파라미터에 대한 reset
        animator.SetBool("IsIDLE", false);
        animator.SetBool("IsMOVE", false);
        //인자로 전달받은 값을 true로
        animator.SetBool(temp, true);
    }

  
}
