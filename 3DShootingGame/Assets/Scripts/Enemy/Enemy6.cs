using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy6 : Enemy
{
    public float rushSpeed = 10f;
    private void Start()
    {
        speed = 5f;
        dir = Vector3.down;
        HP = 12;
        enemyScore = 800;
        InvokeRepeating(nameof(FireBullet), 0f, 0.13f);
        StartCoroutine(RushAfterDelay(3f));

    }

    private void FireBullet()
    {
        float ran = Random.Range(0f, 2f);

        var target = GameObject.FindWithTag("Player");
       
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            for (int i = 0; i < 18; i++)
            {
                CreateBullet(bulletPrefab, firePoint.position, 
                Quaternion.Euler(0, 0, (20 * i) + (int)ran * 10) * bulletDir, () => 10);
            }

        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(FireBullet));
    }

    IEnumerator RushAfterDelay(float delaySeconds)
    {

        yield return new WaitForSeconds(1);
        speed = 0.0f;

        yield return new WaitForSeconds(delaySeconds);

        dir = Vector3.down;
        
        speed = rushSpeed;
    }


}