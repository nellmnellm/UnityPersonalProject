using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Unit
{
    Vector3 pos;
    Quaternion quat;

    protected override void Start()
    {
        base.Start();

        pos = transform.position;
        quat = transform.rotation;

    }

    private void Update()
    {

        StrikeFirst(Spawner.monster_list.ToArray()); //리스트를 배열로


        if (target == null)
        {


            var targetPos = Vector3.Distance(transform.position, pos);

            if (targetPos > 0.1f)
            {
                transform.position
                    = Vector3.MoveTowards(transform.position, pos, Time.deltaTime);
                transform.LookAt(pos);
                SetAnimator("IsMOVE");
            }
            else
            {
                transform.rotation = quat;
                SetAnimator("IsIDLE");
            }
            return;
        }
        //타겟 거리
        var targetDistance = Vector3.Distance(transform.position, target.position);
        //사정 거리에는 들어왔지만 공격 사정 범위에는 포함이 안되는 경우
        if (targetDistance <= T_RANGE && targetDistance > A_RANGE)
        {
            SetAnimator("IsMOVE");
            transform.position = Vector3.MoveTowards(transform.position,
                                                     target.position,
                                                     Time.deltaTime);
        }
        else if (targetDistance <= A_RANGE)
        {
            SetAnimator("IsATTACK");
            
        }
    }
}
