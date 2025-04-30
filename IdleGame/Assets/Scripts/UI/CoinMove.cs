
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
            //�ν����Ϳ� ���� �������� �ʰ� ��ũ��Ʈ�� �߰�.
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

        //�ڷ�ƾ �۵�
        StartCoroutine(Move());
    }

    /// <summary>
    /// �������� ������ => �������� ���� ����Ǵ� ȿ��
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        //�� ��ó �����ϰ� �ֺ��� ������ ��ġ �߰�
        var pos = new Vector2[rects.Length];
        for (int i=0; i<rects.Length; i++)
        {
            pos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-distance, distance);
        }

        //������ ������ ��ġ�� ��Ѹ�
        while (true) 
        {
            for (int i=0;i <rects.Length; i++)
            {
                var rect = rects[i];
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, pos[i], speed * item_move_speed * Time.deltaTime);

                //�Ÿ��� ���� ������ �����ؼ� Ż��.
            }

            if (CheckDistance(pos, 0.5f))
            {
                break;
            }

            yield return null;
        }
        //�۾� ������ ������ �߰�.
        yield return new WaitForSeconds(0.5f);

        //�������� ��� UI������ �����Ű�� ȿ��.

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
