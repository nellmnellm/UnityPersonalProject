using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    /*  private Vector3 direction;
      public void SetDirection(Vector3 dir)
      {
          direction = dir.normalized;
          float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
          transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
          //transform.rotation = Quaternion.Euler(direction);
      } */

    private Func<Vector3> directionFunction;
    private Func<float> speedFunction;

    

    
    public void SetSpeed(Func<float> func)
    {
        speedFunction = func;
    }
    public void SetDirection(Func<Vector3> func)
    {
        directionFunction = func;
    }

    void Update()
    {
        if (directionFunction == null || speedFunction == null) 
            return;

        Vector3 direction = directionFunction().normalized; //���⺤�� �����Լ� ����
        float speed = speedFunction();                      //�ӵ� �����Լ� ����

        // �Ѿ� ȸ�� �ݿ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        transform.position += direction * speed * Time.deltaTime;
    }
}