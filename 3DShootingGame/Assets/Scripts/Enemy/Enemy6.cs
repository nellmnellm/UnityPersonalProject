using System.Collections;
using UnityEngine;
public class Enemy6 : Enemy
{
    public float rushSpeed = 10f;
    private void Start()
    {
        speed = 5f;
        dir = Vector3.down;
        HP = 25;
        enemyScore = 2400;
        InvokeRepeating(nameof(FireBullet), 0f, 0.14f);
        StartCoroutine(RushAfterDelay(3f));
        StartCoroutine(bulletStop(5f));

    }

    protected override void FireBullet()
    {
        base.FireBullet();
        float ran = Random.Range(0f, 2f);

        var target = GameObject.FindWithTag("Player");
       
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            for (int i = 0; i < 18; i++)
            {
                int currentI = i;
                //SetBullet(bulletObjectPool, firePoint.position,
                //() => Quaternion.Euler(0, 0, (20 * currentI) + (int)ran * 10) * bulletDir, () => 10);
                //버그 존재! 오브젝트 풀 사용하면 총알이 의도한 모양대로 나오지 않음
                CreateBullet(bulletPrefab, firePoint.position, 
                () => Quaternion.Euler(0, 0, (20 * currentI) + (int)ran * 10) * bulletDir, () => 10);
            }

        }
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