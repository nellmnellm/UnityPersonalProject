using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public PlayerHealth playerHealth;           //ü��
    public GameObject enemy;                    //����
    public float intervalTime = 10.0f;          //�ݺ� �ð�
    public Transform[] SpawnPools;              //��ȯ ����

    private void Start()
    {
        InvokeRepeating("Spawn", intervalTime, intervalTime);

    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
            return;

        int spawnPointIndex = Random.Range(0, SpawnPools.Length);

        Instantiate(enemy, SpawnPools[spawnPointIndex].position, SpawnPools[spawnPointIndex].rotation, SpawnPools[spawnPointIndex]);
    }
}