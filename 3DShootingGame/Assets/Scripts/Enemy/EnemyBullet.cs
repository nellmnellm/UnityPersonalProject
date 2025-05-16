using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    private Vector3 direction;

    private Func<float> speedFunction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
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