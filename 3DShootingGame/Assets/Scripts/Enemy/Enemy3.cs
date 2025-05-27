using UnityEngine;

public class Enemy3 : Enemy
{
    private void Start()
    {
        speed = 12;
        HP = 7;
        var target = GameObject.FindWithTag("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        enemyScore = 500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.6f);
        StartCoroutine(bulletStop(3.2f));

    }

    protected override void FireBullet()
    {
        base.FireBullet();
        float startTime = Time.time;
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            float TimeSpeed = 15 - (Time.time - startTime) * 2;
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            SetBullet(bulletObjectPool, firePoint.position,
               () => bulletDir,
               () => TimeSpeed);
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, 30f) * bulletDir,
                () => TimeSpeed);
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, -30f) * bulletDir,
                () => TimeSpeed);

          /*  CreateBullet(bulletPrefab, firePoint.position, 
                () => bulletDir, 
                () => TimeSpeed);
            CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, 30f) * bulletDir, 
                () => TimeSpeed);
            CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, -30f) * bulletDir, 
                () => TimeSpeed);
*/
            
        }
    }
   
    
}