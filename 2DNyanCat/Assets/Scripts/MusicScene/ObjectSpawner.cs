using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]


public struct SpawnData
{
    public float time;   
    [Range(-4.2f, 4.2f)]
    public float height; //y�� ��ġ
}

public class ObjectSpawner : MonoBehaviour
{
   // public AudioSource music;
    public Transform spawnPoint; // ȭ�� ������ ��
    public ObjectPool notePool;  // 30��  
    public ObjectPool enemyPool; // 30��


    [SerializeField] private List<SpawnData> enemySpawnData = new List<SpawnData>();
    [SerializeField] private List<SpawnData> noteSpawnData = new List<SpawnData>();
    private int enemySpawnIndex = 0;
    private int noteSpawnIndex = 0; 

    private bool musicStarted = false;
    float startTime;
    private float audioSourceClipLength = 20f; //���߿� ���ְ� ���� double�� �޼��忡 ������

    public GameObject GameOverResult; // ���� ���� UI ȣ��

    void Start()
    {
        /*//���� �������� �ϴ� nulló�� ��
        if (music != null)
        {
            music.PlayDelayed(1f); // ��¦ �����ؼ� ������ ��
            
        }
        //musicStarted = true; */

        startTime = Time.time;
    }

    void Update()
    {
        //!musicStarted || �� ���� ���ǹ��� ������.
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

        // ���� �� ���� (�뷡 ���� ����)
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