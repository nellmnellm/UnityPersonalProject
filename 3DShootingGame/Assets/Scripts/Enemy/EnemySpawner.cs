using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    float currentTime; // ���� �ð�

    public float step = 0.5f; // �ð� ����

    public GameObject enemyFactory; //����� ��ȯ����

    public bool isActive = false;

    public Vector3 MovingVector; //����� �ӵ�

    private void Update()
    {
        if (!isActive) return;

        Move();
        
        currentTime += Time.deltaTime;

        if (currentTime > step)
        {
            var enemy = Instantiate(enemyFactory);
            enemy.transform.position = transform.position;
            enemy.transform.parent = transform;
            currentTime = 0;
        }
        
    }

    public void Move()
    {
        if (MovingVector == null)
            return;
        transform.position += MovingVector * Time.deltaTime;
    }

}
