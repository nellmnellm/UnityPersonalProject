using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private float speed = 5f;
    private float lifetime = 4f; // 예비 제거 기준
    private float activeTime = 0f;
    private ObjectPool pool;

    public void Init(Vector3 startPosition, ObjectPool poolRef)
    {
        transform.position = startPosition;
        activeTime = 0f;
        pool = poolRef;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        activeTime += Time.deltaTime;

        if (activeTime > lifetime || transform.position.x < -10f)
        {
            pool.Return(gameObject);
        }
    }
}