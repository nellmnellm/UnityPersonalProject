using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    float currentTime; // ���� �ð�

    public float step = 0.5f; // �ð� ����

    public GameObject enemyFactory; //����� ��ȯ����

    public bool isActive = false;

    private void Update()
    {
        if (!isActive) return;

        currentTime += Time.deltaTime;

        if (currentTime > step)
        {
            var enemy = Instantiate(enemyFactory);
            enemy.transform.position = transform.position;
            enemy.transform.parent = transform;
            currentTime = 0;
        }
        
    }
}
