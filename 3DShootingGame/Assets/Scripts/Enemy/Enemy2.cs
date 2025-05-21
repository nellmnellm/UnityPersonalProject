using System;
using System.Collections;
using UnityEngine;

public class Enemy2 : Enemy
{
    private void Start()
    {
        speed = 10;
        dir = new Vector3(0.5f, -0.75f, 0); 
        HP = 7;
        enemyScore = 200;
        InvokeRepeating(nameof(FireBullet), 0f, 1f);
        StartCoroutine(bulletStop(4f));
    }

    protected override void FireBullet()
    {
        base.FireBullet();
        StartCoroutine(Littledelay(0.05f));
    }

    IEnumerator Littledelay(float sec)
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            float startTime = Time.time;
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            SetBullet(bulletObjectPool, firePoint.position,
                () => bulletDir,
                () => 16 - 2 * (Time.time - startTime));
            yield return new WaitForSeconds(sec);
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, 5f) * bulletDir,
                () => 12 - 2 * (Time.time - startTime));
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, -5f) * bulletDir,
                () => 12 - 2 * (Time.time - startTime));
            yield return new WaitForSeconds(sec);
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, 10f) * bulletDir,
                () => 8 - 1 * (Time.time - startTime));
            SetBullet(bulletObjectPool, firePoint.position,
                () => bulletDir,
                () => 8 - 1 * (Time.time - startTime));
            SetBullet(bulletObjectPool, firePoint.position,
                () => Quaternion.Euler(0, 0, -10f) * bulletDir,
                () => 8 - 1 * (Time.time - startTime));

           /* CreateBullet(bulletPrefab, firePoint.position, 
                () => bulletDir, 
                ()=>16 - 2 * (Time.time - startTime) );
            yield return new WaitForSeconds(sec);
            CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, 5f) * bulletDir, 
                () => 12 - 2 * (Time.time - startTime) );
            CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, -5f) * bulletDir, 
                () => 12 - 2 * (Time.time - startTime) );
            yield return new WaitForSeconds(sec);
            CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, 10f) * bulletDir, 
                () => 8 - 1 * (Time.time - startTime));
            CreateBullet(bulletPrefab, firePoint.position, 
                () => bulletDir,
                () => 8 - 1 * (Time.time - startTime));
            CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, -10f) * bulletDir, 
                () => 8 - 1 * (Time.time - startTime));*/
        }
    }
}