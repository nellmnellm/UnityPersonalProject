using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target; // �÷��̾��� ��ġ
    Vector3 camera_Offset; // ī�޶�� �÷��̾� �� �Ÿ� ����
    Vector3 moveVector; //ī�޶��� �� ������ �̵� �Ÿ�

    float transition = 0.0f; // ���� ��
    public static float camera_animate_duration = 3.0f; // ī�޶� �̿� ���ϸ��̼� ���� �� ���ӽð�
    public Vector3 animate_offset = new Vector3(0, 5, 5); // �ִϸ��̼� ���� ������
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camera_Offset = transform.position - target.position; // ���� ��
    }

    private void Update()
    {
        moveVector = target.position + camera_Offset;
        moveVector.x = 0; // ī�޶� x�� ����
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);
        
        if (transition > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            //��ȯ�� ������ ����� �۾�
            transform.position = Vector3.Lerp(moveVector + animate_offset, moveVector, transition);
            //�÷��̾��� ��������� ���� �̵��� ����
            transition += Time.deltaTime / camera_animate_duration;
            //��ȯ���� ����
            transform.LookAt(target.position + Vector3.up);
            //���� �Ĵٺ�����
        }
    }
}
