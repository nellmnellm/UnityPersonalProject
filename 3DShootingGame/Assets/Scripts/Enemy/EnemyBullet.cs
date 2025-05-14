using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    public void SetSpeed(float spd)
    {
        speed = spd;   
    }
    
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}