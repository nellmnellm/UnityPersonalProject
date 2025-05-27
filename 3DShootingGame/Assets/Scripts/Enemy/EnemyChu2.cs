using UnityEngine;

public class EnemyChu2 : Enemy
{
   
    private void Start()
    {
        speed = 3;
        dir = Vector3.down;
        HP = 6;
        enemyScore = 500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.12f);
        StartCoroutine(bulletStop(6.2f));
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
            () => Quaternion.Euler(0, 0, 40 * (Time.time - startTime)) * bulletDir,
            () => 20 - (Time.time - startTime) * 5);
            SetBullet(bulletObjectPool, firePoint.position,
            () => Quaternion.Euler(0, 0, -40 * (Time.time - startTime)) * bulletDir,
            () => 20 - (Time.time - startTime) * 5);
        }
    }
}