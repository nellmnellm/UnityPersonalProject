using System;
using System.Collections;
using UnityEngine;

public class EnemyChu2 : Enemy
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
            () => Quaternion.Euler(0, 0, 40 * (Time.time - startTime)) * bulletDir,
            () => 20 - (Time.time - startTime) * 5);
            SetBullet(bulletObjectPool, firePoint.position,
            () => Quaternion.Euler(0, 0, -40 * (Time.time - startTime)) * bulletDir,
            () => 20 - (Time.time - startTime) * 5);
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