using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//게임에 대한 전반적인 설정 관리
public class GameManager : MonoBehaviour
{
    public GameObject Character;
    public Transform Platform_Parent;
    public GameObject Platform;

    public int platform_pos_idx = 0;
    public int character_pos_idx = 0;
    public bool isPlaying = false;
    //배치된 판 리스트
    private List<GameObject> Platform_List = new List<GameObject>();
    //플랫폼 체크 리스트
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
        Debug.Log($"인덱스값 : {idx} / 플랫폼 : {Platform_Check_List[idx]} / 방향 : {direction}");
        //방향이 맞을 경우
        if (Platform_Check_List[idx % 20] == direction)
        {

            //캐릭터의 위치 변경
            character_pos_idx++;
            Character.transform.position = Platform_List[idx].transform.position
                + new Vector3(0f, 0.4f, 0f);

            //바닥 설정
            NextFlatform(platform_pos_idx);

        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("게임 오버");
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
            else //인덱스 범위를 넘은 경우
            {
                //왼쪽 발판
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
                //오른쪽 발판
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
