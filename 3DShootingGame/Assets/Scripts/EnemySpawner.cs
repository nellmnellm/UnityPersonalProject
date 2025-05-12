using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    float currentTime; // 현재 시간

    public float step = 0.5f; // 시간 간격

    public GameObject enemyFactory; //어떤몹을 소환할지

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
