using System.Collections;
using UnityEngine;

public class Boss1 : Enemy
{


    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        HP = 300;
        enemyScore = 10000;
        StartCoroutine(AfterStop(1f));
        InvokeRepeating(nameof(FireBullet), 0f, 0.1f);
    }


    void FireBullet()
    {
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            for (int i=5; i<10; i++)
            {
                

                for (int j = 0; j < 4; j++)
                {

                    Vector3 TanbulletDir = Quaternion.Euler(0, 0, j * 90) * bulletDir;

                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    bullet.GetComponent<EnemyBullet>().SetDirection(TanbulletDir);
                    bullet.GetComponent<EnemyBullet>().SetSpeed( 3 * i );
                }
            }
            
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }

    IEnumerator AfterStop(float delaySeconds)
    {

        yield return new WaitForSeconds(delaySeconds);
        speed = 0.0f;
    }

}