using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region
    //1. 몬스터의 생성은, 프레임 당 생성보다는 초 간격이 좋음 (젠 타임)
    //2. 이 작업을 코루틴이라는 기법으로 설계함. yield
    //3. 코루틴이 자주 사용되는 경우.
    //    -1. 몬스터 생성.
    //    -2. 물약, 스킬 쿨타임. 
    // 특정 패턴을 유발하는
    // IEnumerator 열거자.
    #endregion

    //for문에서 얼마나 많이/자주 respawn할지의 갯수 고민
    public int count;
    public float spawnTime;
    //public GameObject monster_prefab; // 몬스터의 프리팹

    public static List<Monster> monster_list = new List<Monster>();
    public static List<Player> player_list = new List<Player>();
    private void Start()
    {
        StartCoroutine(CSpawn());
    }


    IEnumerator CSpawn()
    {
        Vector3 playerVector3 = new Vector3(5, 0, -5);
        // 1.어디에 생성?
        Vector3 pos;
        // 2. 몇 회 생성?
        for (int i = 0; i < count; i++)
        {
            Vector2 randomcircle = Random.insideUnitCircle;
            pos = playerVector3 + new Vector3(randomcircle.x, 0, randomcircle.y) * 5.0f * Random.Range(1f, 1.5f);
            // 3. 어떤 형태로 생성?
            /*pos = playerVector3 + Random.insideUnitSphere * 5.0f * Random.Range(1f, 5f);
            pos.y = 0.0f;
*/
            //Instantiate(monster_prefab, pos, Quaternion.identity); //고유값으로 회전0을 줌
            var go = Manager.Pool.pooling("Monster").get((value)=>
            {
                value.GetComponent<Monster>().MonsterInit();
                value.transform.position = pos;
                value.transform.LookAt(playerVector3);
                /*var go = value.GetComponent<Monster>();
                monster_list.Add(go);*/
            });

            //StartCoroutine(CRelease(go));
        }
        // yield return : 일정 시점 후 다시 돌아오는 코드.
        // WaitForSeconds(float t) : 작성한 값만큼 대기합니다.
        yield return new WaitForSeconds(spawnTime);
        StartCoroutine(CSpawn());
    }
    

    IEnumerator CRelease(GameObject obj)
    {
        yield return new WaitForSeconds(5.0f);

        Manager.Pool.pool_dict["Monster"].Release(obj);
    }
}

    
