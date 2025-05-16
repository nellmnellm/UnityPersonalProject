using System;
using System.Collections;
using UnityEngine;

public class Enemy2 : Enemy
{
    private void Start()
    {
        speed = 10;
        dir = new Vector3(0.5f, -0.75f, 0); 
        HP = 5;
        enemyScore = 200;
        InvokeRepeating(nameof(FireBullet), 0f, 1f);
    }
    
    private void FireBullet()
    {
        StartCoroutine(Littledelay(0.05f));
    }

    IEnumerator Littledelay(float sec)
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            
            CreateBullet(bulletPrefab, firePoint.position, bulletDir, ()=>15);
            yield return new WaitForSeconds(sec);
            CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 5f) * bulletDir, () => 11);
            CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -5f) * bulletDir, () => 11);
            yield return new WaitForSeconds(sec);
            CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 10f) * bulletDir, () => 7);
            CreateBullet(bulletPrefab, firePoint.position, bulletDir, () => 7);
            CreateBullet(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -10f) * bulletDir, () => 7);
        }
    }
    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}