using UnityEngine;

public class Spiral : MonoBehaviour
{
    public GameObject prefab; //������Ʈ
    public int count = 50;    //����
    public float degree = 10; //�ѹ��� ����
    public float radius = 0.1f; //�ѹ��� ������

    void Start()
    {
        float d = 0.0f; //���� ����
        float r = 0.0f; //���� ������

        for (int i = 0; i < count; i++) //ī��Ʈ��ŭ �۾�
        {
            var radian = d * Mathf.Deg2Rad; // ���� 

            var x = Mathf.Cos(radian) * r;   //x
            var y = Mathf.Sin(radian) * r;   //y

            var pos = transform.position + new Vector3(x, y, 0); //��� �� �����ֱ�
            var go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.parent = transform;

            d += degree; //������ �ѹ� �� ���� �߰�
            r += radius; //������ �ѹ� �� ���� �߰� 
        }
    }



}