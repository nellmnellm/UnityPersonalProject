using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region
    //1. ������ ������, ������ �� �������ٴ� �� ������ ���� (�� Ÿ��)
    //2. �� �۾��� �ڷ�ƾ�̶�� ������� ������. yield
    //3. �ڷ�ƾ�� ���� ���Ǵ� ���.
    //    -1. ���� ����.
    //    -2. ����, ��ų ��Ÿ��. 
    // Ư�� ������ �����ϴ�
    // IEnumerator ������.
    #endregion

    //for������ �󸶳� ����/���� respawn������ ���� ���
    public int count;
    public float spawnTime;
    public GameObject monster_prefab; // ������ ������


    private void Start()
    {
        StartCoroutine(CSpawn());
    }
    IEnumerator CSpawn()
    {
        Vector3 playerVector3 = new Vector3(5, 0, -5);
        // 1.��� ����?
        Vector3 pos;
        // 2. �� ȸ ����?
        for (int i = 0; i < count; i++)
        {
            Vector2 randomcircle = Random.insideUnitCircle;
            pos = playerVector3 + new Vector3(randomcircle.x, 0, randomcircle.y) * 5.0f * Random.Range(1f, 5f);
            // 3. � ���·� ����?
            /* pos = playerVector3 + Random.insideUnitSphere * 5.0f * Random.Range(1f, 5f);
             pos.y = 0.0f;*/

            Instantiate(monster_prefab, pos, Quaternion.identity); //���������� ȸ��0�� ��
        }
        // yield return : ���� ���� �� �ٽ� ���ƿ��� �ڵ�.
        // WaitForSeconds(float t) : �ۼ��� ����ŭ ����մϴ�.
        yield return new WaitForSeconds(spawnTime);
        StartCoroutine(CSpawn());
    }
}

    
