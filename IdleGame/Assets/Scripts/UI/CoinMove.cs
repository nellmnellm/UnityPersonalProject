
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CoinMove : MonoBehaviour
{
    Vector3 target;
    RectTransform[] rects = new RectTransform[5];

    [Header("Move")]
    public float distance;
    public float speed;
    public float item_move_speed = 50;


    private void Awake()
    {
        for (int i=0; i<rects.Length; i++)
        {
            rects[i] = transform.GetChild(i).GetComponent<RectTransform>();
            //인스펙터에 직접 연결하지 않고 스크립트로 추가.
        }
    }
    public void Init(Vector3 pos)
    {
        target = pos;
        transform.position = Camera.main.WorldToScreenPoint(pos);

        for (int i = 0; i < rects.Length; i++)
        {
            rects[i].anchoredPosition = Vector2.zero;
        }
        transform.parent = B_Canvas.Instance.GetLayer(0);

        //코루틴 작동
        StartCoroutine(Move());
    }

    /// <summary>
    /// 아이템이 퍼지고 => 아이템이 돈에 흡수되는 효과
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        //적 근처 랜덤하게 주변에 아이템 위치 추가
        var pos = new Vector2[rects.Length];
        for (int i=0; i<rects.Length; i++)
        {
            pos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-distance, distance);
        }

        //정해진 아이템 위치로 흩뿌림
        while (true) 
        {
            for (int i=0;i <rects.Length; i++)
            {
                var rect = rects[i];
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, pos[i], speed * item_move_speed * Time.deltaTime);

                //거리에 대한 로직을 설계해서 탈출.
            }

            if (CheckDistance(pos, 0.5f))
            {
                break;
            }

            yield return null;
        }
        //작업 끝나고 딜레이 추가.
        yield return new WaitForSeconds(0.5f);

        //아이템을 상단 UI쪽으로 흡수시키는 효과.

        while(true)
        {
            for (int i=0; i< rects.Length; i++)
            {
                var rect = rects[i];
                rect.position = Vector2.MoveTowards(rect.position, B_Canvas.Instance.Coin.position, speed * item_move_speed * Time.deltaTime);
            }

            if (CheckDistanceCoinUI(0.5f))
            {
                Manager.Pool.pool_dict["CoinMove"].Release(gameObject);
                break;
            }

            yield return null;
        }
        
    }
    private bool CheckDistanceCoinUI(float range)
    {
        for (int i = 0; i < rects.Length; i++)
        {
            var distance = Vector2.Distance(rects[i].anchoredPosition,
                B_Canvas.Instance.Coin.position);
            if (distance > range)
                return false;

            
        }
        return true;
    }

    private bool CheckDistance(Vector2[] end, float range)
    {
        for (int i=0; i < rects.Length; i++)
        {
            var distance = Vector2.Distance(rects[i].anchoredPosition, end[i]);

            if (distance > range)
            {
                return false;
            }

        }
        return true;
    }
}
