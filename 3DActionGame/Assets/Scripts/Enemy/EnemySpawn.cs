using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public PlayerHealth playerHealth;           //체력
    public GameObject enemy;                    //몬스터
    public float intervalTime = 10.0f;          //반복 시간
    public Transform[] SpawnPools;              //소환 지점

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