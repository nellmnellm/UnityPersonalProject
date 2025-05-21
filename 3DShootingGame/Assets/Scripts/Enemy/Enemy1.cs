using System;
using UnityEngine;

public class Enemy1 : Enemy
{
    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        HP = 7;
        enemyScore = 100;
        InvokeRepeating(nameof(FireBullet), 0f, 0.5f);
        StartCoroutine(bulletStop(5f));
    }

    protected override void FireBullet()
    {
        base.FireBullet();
        float startTime = Time.time;

        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            SetBullet(bulletObjectPool, firePoint.position,
                () => bulletDir,
                () => 2f + (Time.time - startTime) * 13);


         /*   CreateBullet(bulletPrefab, firePoint.position, 
                () => bulletDir, 
                () => 2f + (Time.time - startTime) * 13);
            */
        }
    }

    
}