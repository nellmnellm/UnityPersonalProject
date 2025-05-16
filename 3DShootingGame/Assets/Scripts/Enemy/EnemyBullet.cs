using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    private Vector3 direction;

    private Func<float> speedFunction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        //transform.rotation = Quaternion.Euler(direction);
    }

    
    public void SetSpeed(Func<float> func)
    {
        speedFunction = func;
    }

    void Update()
    {
        float speed = speedFunction != null ? speedFunction() : 0f;
        transform.position += direction * speed * Time.deltaTime;
    }
}