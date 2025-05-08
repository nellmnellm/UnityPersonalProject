using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Ÿ�� ���� -> �÷��̾� �̵� -> Ÿ���� ���� (������ Ÿ�� ������ �����ϰ�)
public class TileManager : MonoBehaviour
{
    [Header("Ÿ��")]
    public GameObject[] tilePrefabs; //����� Ÿ��
    private List<GameObject> tiles; //Ÿ�� ����Ʈ

    
    private Transform player_transform; // �÷��̾� ��ġ
    private float spawnZ = 0.0f; //����(Z��) 
    private float tileLength = 8.0f; //Ÿ���� ����
    private float passZone = 15.0f; //Ÿ�� ���� �Ÿ�
    private int tile_on_screen = 7; //ȭ�鿡 ��ġ�� Ÿ��

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
        var go = Instantiate(tilePrefabs[randInt]); //�����Ȱ� ����
        go.transform.SetParent(transform); //�ڽĿ�����Ʈ��
        go.transform.position = Vector3.forward * spawnZ + new Vector3 (0,-0.5f,0);
        spawnZ += tileLength; // Ÿ�� ���� ���� ������ġ ����
        tiles.Add(go); //Ÿ�� ����Ʈ�� ���
    }

    private void Release()
    {
        Destroy(tiles[0]); //Ÿ�� ����
        tiles.RemoveAt(0); //Ÿ�� ����Ʈ������ ����
    }
}
