using UnityEngine;

public class EnemyChu3 : Enemy
{
    int bulletCount = 24;
    float radius = 7;
    private void Start()
    {
        speed = 3f;
        dir = Vector3.down;
        HP = 48;
        enemyScore = 2500;
        InvokeRepeating(nameof(FireBullet), 0f, 1.3f);
        StartCoroutine(bulletStop(5));
    }

    protected override void FireBullet()
    {
        base.FireBullet();
        var target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            Vector3 bulletDir = (target.transform.position - firePoint.position).normalized;
            for (int i = 0; i < bulletCount; i++)
            {
                int CurrentI = i;
                float angle = 360f / bulletCount * i;
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius; //5´Â ¹Ý°æ
                Vector3 bulletPos = firePoint.position + offset;

                SetBullet(bulletObjectPool, bulletPos,
                    () => bulletDir,
                    () => 12);
            }
        }
    }
}