using UnityEngine;

public class Enemy3 : Enemy
{
    private void Start()
    {
        speed = 15;
        HP = 5;
        var target = GameObject.FindWithTag("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        enemyScore = 500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.5f);

    }

    void FireBullet()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetDirection(bulletDir);
            bullet.GetComponent<EnemyBullet>().SetSpeed(20);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}