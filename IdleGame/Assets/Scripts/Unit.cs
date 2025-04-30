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

    [Header("타겟 위치")]
    public Transform target;
    public Transform attack_transform;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        //코드 내에서 animator를 인식하고 필드나 메서드 사용가능.
    }

    protected virtual void AttackObject()
    {
        Debug.Log("이벤트 테스트");
        Manager.Pool.pooling("Attack").get((value) =>
        {
            value.transform.position = attack_transform.position;
            value.GetComponent<Attack>().Init(target, 1, "Attack01");
           
        });
    }
           


    
    protected void SetAnimator(string temp)
    {
        // bool타입이 아닌 Trigger 타입은 이렇게 처리
        if (temp == "IsATTACK")
        {
            animator.SetTrigger("IsATTACK");
            return;
        }
        //기본 파라미터에 대한 reset
        animator.SetBool("IsIDLE", false);
        animator.SetBool("IsMOVE", false);
        //인자로 전달받은 값을 true로
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
                //가장 가까운 거리
                closet = enemys[i].transform;
                //최대 거리는 타겟의 거리로 설정.
                max = target_distance;
            }
            

        }
        //가장 가까운 값을 타겟으로.
        target = closet;
        if (target != null)
        {
            transform.LookAt(target.position);
        }
    }
  
}
