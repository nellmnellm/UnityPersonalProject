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

    void FireBullet()
    {
        StartCoroutine(Littledelay(0.1f));
    }

    IEnumerator Littledelay(float sec)
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            Vector3 bulletDir5do = Quaternion.Euler(0, 0, 5f) * bulletDir;
            Vector3 bulletDirm5do = Quaternion.Euler(0, 0, -5f) * bulletDir;
            GameObject bullet1 = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet1.GetComponent<EnemyBullet>().SetDirection(bulletDir);
            bullet1.GetComponent<EnemyBullet>().SetSpeed(15);
            yield return new WaitForSeconds(sec);
            GameObject bullet2 = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet2.GetComponent<EnemyBullet>().SetDirection(bulletDir5do);
            bullet2.GetComponent<EnemyBullet>().SetSpeed(10);
            GameObject bullet3 = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet3.GetComponent<EnemyBullet>().SetDirection(bulletDirm5do);
            bullet3.GetComponent<EnemyBullet>().SetSpeed(10);

        }
    }
    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }


}