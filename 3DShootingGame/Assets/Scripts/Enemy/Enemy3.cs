using System;
using UnityEngine;

public class Enemy3 : Enemy
{
    private void Start()
    {
        speed = 15;
        HP = 5;
        var target = GameObject.FindWithTag("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        enemyScore = 500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.6f);

    }
    
    private void FireBullet()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            CreateBullet(bulletPrefab, firePoint.position, bulletDir, () => 15);
            CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 30f) * bulletDir, () => 15);
            CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -30f) * bulletDir, () => 15);
            
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}