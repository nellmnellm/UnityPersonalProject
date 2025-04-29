using UnityEngine;
using UnityEngine.UI;

public class HitText : MonoBehaviour
{
    Vector3 target; //���
    
    
    public Text message; //�ؽ�Ʈ
    //�ؽ�Ʈ��� ��ġ ������
    float up = 1f;

    private void Start()
    {
        
    }

    private void Update()
    {
        var pos = new Vector3(target.x, target.y + up, target.z);
        transform.position = Camera.main.WorldToScreenPoint(pos);
        up += Time.deltaTime; //�ð��� ���� �������� �ø��°�
    }
    public void Init(Vector3 pos, double value)
    {
        target = pos;
        message.text = value.ToString();
        //UI�� �⺻ ķ�۽��� ����
        transform.parent = B_Canvas.Instance.transform;
        //
        //Release();
    }
    //�Ϲ� �������� ũ��Ƽ�� ������ ����

    private void Release()
    {
        Manager.Pool.pool_dict["Hit"].Release(gameObject);

    }
}
