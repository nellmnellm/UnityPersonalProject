using System;
using System.Collections;
using UnityEngine;

public class EnemyChu1 : Enemy
{
   
    private void Start()
    {
        speed = 3;
        dir = Vector3.down;
        HP = 6;
        enemyScore = 500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.09f);
        StartCoroutine(bulletStop(6.2f));
    }

    private void FireBullet()
    {
        float startTime = Time.time;
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            SetBullet(bulletObjectPool, firePoint.position,
            () => Quaternion.Euler(0, 0, 30 - 25 * (Time.time - startTime)) * bulletDir,
            () => (Time.time - startTime) * 20);
            SetBullet(bulletObjectPool, firePoint.position,
            () => Quaternion.Euler(0, 0, -30 + 25 * (Time.time - startTime)) * bulletDir,
            () => (Time.time - startTime) * 20);

            /*  CreateBullet(bulletPrefab, firePoint.position,
                  () => Quaternion.Euler(0,0, Time.time - startTime  * 360f) * Vector3.down,
                  () => (Time.time - startTime) * 8);
      */
        }
    }
    IEnumerator bulletStop(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        CancelInvoke(nameof(FireBullet));

    }
    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}