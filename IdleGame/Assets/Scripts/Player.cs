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

        StrikeFirst(Spawner.monster_list.ToArray()); //����Ʈ�� �迭��


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
        //Ÿ�� �Ÿ�
        var targetDistance = Vector3.Distance(transform.position, target.position);
        //���� �Ÿ����� �������� ���� ���� �������� ������ �ȵǴ� ���
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
