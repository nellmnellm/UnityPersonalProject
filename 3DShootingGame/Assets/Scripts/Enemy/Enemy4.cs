using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy4 : Enemy
{
    public float rushSpeed = 30f;
    private void Start()
    {
        speed = 5f;
        dir = Vector3.down;
        HP = 120;
        enemyScore = 2000;
        InvokeRepeating(nameof(FireBullet), 0f, 0.2f);
        StartCoroutine(RushAfterDelay(9f));

    }
    
    private void FireBullet()
    {
        var target = GameObject.FindWithTag("Player");
        
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            for (int i=0; i<30; i++)
            {
                CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, (12 * i)) * bulletDir, () => 6);
            }        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }

    IEnumerator RushAfterDelay(float delaySeconds)
    {
        
        yield return new WaitForSeconds(1);
        speed = 0.0f;
        yield return new WaitForSeconds(delaySeconds);

        var target = GameObject.FindWithTag("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        speed = rushSpeed;
    }

    
}