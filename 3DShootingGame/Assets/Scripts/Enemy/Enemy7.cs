using UnityEngine;

public class Enemy7 : Enemy
{
   
    private void Start()
    {
        speed = 2;
        dir = Vector3.down;
        HP = 60;
        enemyScore = 2000;
        InvokeRepeating(nameof(FireBullet), 0f, 0.02f);
        StartCoroutine(bulletStop(8f));
    }

    protected override void FireBullet()
    {
        base.FireBullet();
        float startTime = Time.time;

        SetBullet(bulletObjectPool, firePoint.position,
            () => Quaternion.Euler(0, 0, Time.time - startTime * 360f) * Vector3.down,
            () => (Time.time - startTime) * 8);

      /*  CreateBullet(bulletPrefab, firePoint.position,
            () => Quaternion.Euler(0,0, Time.time - startTime  * 360f) * Vector3.down,
            () => (Time.time - startTime) * 8);
*/
        
    }
    
}