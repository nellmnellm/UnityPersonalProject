using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]


public struct SpawnData
{
    public float time;   
    [Range(-4.2f, 4.2f)]
    public float height; //y축 위치
}

public class ObjectSpawner : MonoBehaviour
{
   // public AudioSource music;
    public Transform spawnPoint; // 화면 오른쪽 끝
    public ObjectPool notePool;  // 30개  
    public ObjectPool enemyPool; // 30개


    [SerializeField] private List<SpawnData> enemySpawnData = new List<SpawnData>();
    [SerializeField] private List<SpawnData> noteSpawnData = new List<SpawnData>();
    private int enemySpawnIndex = 0;
    private int noteSpawnIndex = 0; 

    private bool musicStarted = false;
    float startTime;
    private float audioSourceClipLength = 20f; //나중에 없애고 직접 double로 메서드에 넣을것

    public GameObject GameOverResult; // 게임 끝난 UI 호출

    void Start()
    {
        /*//추후 넣을꺼라서 일단 null처리 ★
        if (music != null)
        {
            music.PlayDelayed(1f); // 살짝 지연해서 안정성 ↑
            
        }
        //musicStarted = true; */

        startTime = Time.time;
    }

    void Update()
    {
        //!musicStarted || 는 추후 조건문에 넣을것.
        if ( enemySpawnIndex >= enemySpawnData.Count && noteSpawnIndex >= noteSpawnData.Count)   
            return;

        float songTime = Time.time - startTime; //music.time;

        if (enemySpawnIndex < enemySpawnData.Count && songTime >= enemySpawnData[enemySpawnIndex].time)
        {
            SpawnEnemy(enemySpawnData[enemySpawnIndex].height);
            enemySpawnIndex++;
        }

        if (noteSpawnIndex < noteSpawnData.Count && songTime >= noteSpawnData[noteSpawnIndex].time)
        {
            
            SpawnNote(noteSpawnData[noteSpawnIndex].height);
            noteSpawnIndex++;
        }

        // 종료 시 로직 (노래 길이 이후)
        if (songTime >= audioSourceClipLength)
        {
            Debug.Log("Song ended.");
            GameOverResult.SetActive(true);
            enabled = false;
        }
    }

    private void SpawnNote(float height)
    {
        GameObject note = notePool.Get();
        if (note != null)
        {
            PooledObject pooled = note.GetComponent<PooledObject>();
            pooled.Init(spawnPoint.position + new Vector3(0, height, 0) , notePool);
        }
    }

    private void SpawnEnemy(float height)
    {
        GameObject enemy = enemyPool.Get();
        if (enemy != null)
        {
            PooledObject pooled = enemy.GetComponent<PooledObject>();
            pooled.Init(spawnPoint.position + new Vector3(0, height, 0), enemyPool);
        }
    }
}