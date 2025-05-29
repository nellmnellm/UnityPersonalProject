using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public List<TowerData> towerPool;

    public Transform TowerParent;

    public void RandSpawnTower (Vector2 spawnPos)
    {
        TowerData towerData = GetTower();

        if (GameManager.instance.Cost(towerData.cost))
        {
            GameObject tower = Instantiate(towerData.prefab, spawnPos, Quaternion.identity);
            Debug.Log($"{towerData.towerName}타워가 생성되었스빈다");
        }
        else
        {
            Debug.Log($"잔액이 부족합니다");
        }
    }

    private TowerData GetTower()
    {
        float totalWeight = 0;
        foreach (TowerData data in towerPool)
        {
            totalWeight += data.weight;
        }

      
        float rand = Random.Range(0, totalWeight);
        float current = 0;

        foreach (TowerData tower in towerPool)
        {
            current += tower.weight;
            if (rand <= current)
            {
                return tower;
            }
        }
        return towerPool[0];
    }
}
