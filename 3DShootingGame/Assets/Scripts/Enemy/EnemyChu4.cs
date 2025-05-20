using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyChu4 : Enemy
{
    
    private void Start()
    {
        speed = 3.5f;
        dir = Vector3.down;
        HP = 50;
        enemyScore = 2500;
        InvokeRepeating(nameof(FireBullet), 0f, 1.1f);
        StartCoroutine(bulletStop(5));
    }

    private void FireBullet()
    {
        var target = GameObject.FindWithTag("Player");
        if (target == null) return;

        Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
        float spacing = 1.6f; 

        for (int i = -3; i <= 3; i++)
        {
            int CurrentI = i;
            for (int j = -3; j <= 3; j++)
            {
                int CurrentJ = j;
                Vector3 offset = new Vector3(CurrentI * spacing, CurrentJ * spacing, 0f);
                Vector3 spawnPos = firePoint.position + offset;
                

                SetBullet(bulletObjectPool, spawnPos,
                    () => bulletDir,
                    () => 12f); // ¼Óµµ
            }
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