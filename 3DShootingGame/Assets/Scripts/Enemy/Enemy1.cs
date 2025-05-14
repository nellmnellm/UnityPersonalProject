using UnityEngine;

public class Enemy1 : Enemy
{
    

    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        HP = 5;
        enemyScore = 100;
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
            bullet.GetComponent<EnemyBullet>().SetSpeed(15);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }
}