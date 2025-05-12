using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    float currentTime; // ���� �ð�

    public float step = 0.2f; // �ð� ����

    public GameObject emenyFactory; //����� ��ȯ����

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > step)
        {
            var enemy = Instantiate(emenyFactory);
            enemy.transform.position = transform.position;
            enemy.transform.parent = transform;
            currentTime = 0;
        }
        
    }
}
