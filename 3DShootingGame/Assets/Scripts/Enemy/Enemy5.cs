using System.Collections;
using UnityEngine;

public class Enemy5 : Enemy
{


    public float rushSpeed = 10f;
    public int starCount = 10;
    public float starRadius = 5f;
    private void Start()
    {
        speed = 5f;
        dir = Vector3.down;
        HP = 90;
        enemyScore = 2500;
        InvokeRepeating(nameof(FireBullet), 0f, 0.4f);
        StartCoroutine(RushAfterDelay(5f));
        StartCoroutine(bulletStop(6f));
    }

    protected override void FireBullet()
    {
        base.FireBullet();
        FireStarPattern();
    }
   
    IEnumerator RushAfterDelay(float delaySeconds)
    {

        yield return new WaitForSeconds(1);
        speed = 0.05f;

        yield return new WaitForSeconds(delaySeconds);

        var target = GameObject.FindWithTag("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        speed = rushSpeed;
    }

    void FireStarPattern()
    {
        GameObject target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            SpawnStarPattern(transform.position, starCount, starRadius, targetDir, bulletPrefab);
        }
    }
    public void SpawnStarPattern(Vector3 center, int count, float radius, Vector3 targetDir, GameObject bulletPrefab)
    {
        float smallRadius = radius * 0.382f;
        float middleRadius = radius * 0.691f;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, targetDir.normalized); // 회전 방향

        for (int i = 0; i < count; i++)
        {
            float radian = i * Mathf.PI * 2 / count;
            float radian2 = i * Mathf.PI * 2 / count + Mathf.PI / count;

            Vector3 offset;
            Vector3 offset2;

            if (i % 2 == 0)
            {
                offset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0) * radius;
            }
            else
            {
                offset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0) * smallRadius;
            }

            offset2 = new Vector3(Mathf.Cos(radian2), Mathf.Sin(radian2), 0) * middleRadius;

            Vector3 pos1 = center + rotation * offset;
            Vector3 pos2 = center + rotation * offset2;

            Vector3 dir1 = (pos1 - center).normalized;
            Vector3 dir2 = (pos2 - center).normalized;

            GameObject b1 = Instantiate(bulletPrefab, pos1, Quaternion.identity);
            GameObject b2 = Instantiate(bulletPrefab, pos2, Quaternion.identity);

           

            b1.GetComponent<EnemyBullet>().SetDirection(()=>dir1);
            b1.GetComponent<EnemyBullet>().SetSpeed(() => 8);
            b2.GetComponent<EnemyBullet>().SetDirection(()=>dir2);
            b2.GetComponent<EnemyBullet>().SetSpeed(() => 8);
        }
    }
}