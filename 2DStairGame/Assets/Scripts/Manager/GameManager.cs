using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//���ӿ� ���� �������� ���� ����
public class GameManager : MonoBehaviour
{
    public GameObject Character;
    public Transform Platform_Parent;
    public GameObject Platform;

    public int platform_pos_idx = 0;
    public int character_pos_idx = 0;
    public bool isPlaying = false;
    //��ġ�� �� ����Ʈ
    private List<GameObject> Platform_List = new List<GameObject>();
    //�÷��� üũ ����Ʈ
    private List<int> Platform_Check_List = new List<int>();

    private void Start()
    {
        SetFlatform();
        Init();
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CheckPlatform(character_pos_idx, 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CheckPlatform(character_pos_idx, 0);
            }

        }


    }

    private void CheckPlatform(int idx, int direction)
    {
        Debug.Log($"�ε����� : {idx} / �÷��� : {Platform_Check_List[idx]} / ���� : {direction}");
        //������ ���� ���
        if (Platform_Check_List[idx % 20] == direction)
        {

            //ĳ������ ��ġ ����
            character_pos_idx++;
            Character.transform.position = Platform_List[idx].transform.position
                + new Vector3(0f, 0.4f, 0f);

            //�ٴ� ����
            NextFlatform(platform_pos_idx);

        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("���� ����");
        isPlaying = false;
    }

    private void SetFlatform()
    {
        for(int i=0; i<20; i++)
        {
            var platform = Instantiate(Platform, Vector3.zero, Quaternion.identity, Platform_Parent);
            Platform_List.Add(platform);
            Platform_Check_List.Add(0);
        }
    }


    private void Init()
    {
        Character.transform.position = Vector3.zero;

        for (platform_pos_idx =0; platform_pos_idx < 20; platform_pos_idx++)
        {
            NextFlatform(platform_pos_idx);
        }

        character_pos_idx = 0;
        isPlaying = true;
    }

    private void NextFlatform(int idx)
    {
        int pos = UnityEngine.Random.Range(0, 2);

        if (idx == 0)
        {
            //Platform_Check_List[idx] = pos;
            Platform_List[idx].transform.position = new Vector3(0, -0.5f, 0);
        }
        else
        {
            if (platform_pos_idx < 20)
            {
                if (pos == 0)
                {
                    Platform_Check_List[idx] = pos;
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(-1, 0.5f, 0);
                }
                else
                {
                    Platform_Check_List[idx] = pos;
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(1, 0.5f, 0);
                }
            }
            else //�ε��� ������ ���� ���
            {
                //���� ����
                if (pos == 0)
                {
                    if (idx % 20 == 0)
                    {
                        Platform_Check_List[19] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[19].transform.position + new Vector3(-1, 0.5f, 0);
                    }
                    else
                    {
                        Platform_Check_List[idx % 20] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[idx % 20].transform.position + new Vector3(-1, 0.5f, 0);
                    }
                }
                //������ ����
                else
                {
                    if (idx % 20 == 0)
                    {
                        Platform_Check_List[19] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[19].transform.position + new Vector3(1, 0.5f, 0);
                    }
                    else
                    {
                        Platform_Check_List[idx % 20] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[idx % 20].transform.position + new Vector3(1, 0.5f, 0);
                    }
                }
            }
        }
    }
}
