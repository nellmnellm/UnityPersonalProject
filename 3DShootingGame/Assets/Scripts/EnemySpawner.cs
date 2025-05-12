using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    float currentTime; // 현재 시간

    public float step = 0.2f; // 시간 간격

    public GameObject emenyFactory; //어떤몹을 소환할지

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
