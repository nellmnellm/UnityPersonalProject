using System;
using UnityEngine;

public class Enemy1 : Enemy
{
    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        HP = 5;
        enemyScore = 100;
        InvokeRepeating(nameof(FireBullet), 0f, 0.5f);
    }

    private void FireBullet()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            CreateBullet(bulletPrefab, firePoint.position, bulletDir, () => 15);
            
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}