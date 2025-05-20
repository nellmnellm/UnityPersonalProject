using System;
using UnityEngine;

public class EnemyChu : Enemy
{
   
    private void Start()
    {
        speed = 2;
        dir = Vector3.down;
        HP = 60;
        enemyScore = 500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.01f);
    }

    private void FireBullet()
    {
        float startTime = Time.time;
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            SetBullet(bulletObjectPool, firePoint.position,
            () => bulletDir ,
            () => (Time.time - startTime));

      /*  CreateBullet(bulletPrefab, firePoint.position,
            () => Quaternion.Euler(0,0, Time.time - startTime  * 360f) * Vector3.down,
            () => (Time.time - startTime) * 8);
*/
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}