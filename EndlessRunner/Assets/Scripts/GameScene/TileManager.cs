using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// 타일 생성 -> 플레이어 이동 -> 타일을 제거 (월드의 타일 개수가 균일하게)
public class TileManager : MonoBehaviour
{
    [Header("타일")]
    public GameObject[] tilePrefabs; //등록할 타일
    private List<GameObject> tiles; //타일 리스트

    
    private Transform player_transform; // 플레이어 위치
    private float spawnZ = 0.0f; //스폰(Z축) 
    private float tileLength = 8.0f; //타일의 길이
    private float passZone = 15.0f; //타일 유지 거리
    private int tile_on_screen = 7; //화면에 배치할 타일

    private void Start()
    {
        tiles = new List<GameObject>();
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i=0; i<tile_on_screen; i++)
        {
            Spawn();
        }
    }

    private void Update()
    {
        if (player_transform.position.z - passZone > (spawnZ - tile_on_screen * tileLength))
        {
            Spawn();
            Release();
        }
    }

   

    private void Spawn() //int prefabIdx = -1)
    {
        int randInt = Random.Range(0, 4);
        var go = Instantiate(tilePrefabs[randInt]); //고정된값 생성
        go.transform.SetParent(transform); //자식오브젝트로
        go.transform.position = Vector3.forward * spawnZ + new Vector3 (0,-0.5f,0);
        spawnZ += tileLength; // 타일 길이 기준 생성위치 증가
        tiles.Add(go); //타일 리스트에 등록
    }

    private void Release()
    {
        Destroy(tiles[0]); //타일 제거
        tiles.RemoveAt(0); //타일 리스트에서도 제거
    }
}
