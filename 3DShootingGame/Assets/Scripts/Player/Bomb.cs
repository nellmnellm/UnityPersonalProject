using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}