using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;



    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0);

        // �⺻ �̵� �ӵ� ����
        float moveSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                          ? 3f : speed;

        // �̵�
        transform.position += dir * moveSpeed * Time.deltaTime;

        // ��ġ ���� ����
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -9f, 9f);
        pos.y = Mathf.Clamp(pos.y, -15f, 12f);
        transform.position = pos;
    }
    // transform.Translate(Vector3 dir);
    // ���� ������Ʈ�� �̵���Ű�� ���� �뵵
    // ���� ������Ʈ�� ��ġ�� Vector3 �������� �̵��ϰ� ��
    // (���� ������ ���õ� ������ ���������� �ʰ� �ܼ��� �̵� ������� ����)
    // --> �⺻���� ������

    // transform.position�� ���� ����ص� ��ġ�� ������Ʈ�� position�� �ٲ� �� ����
    // --> �ַ� ��Ż ���� ������ ������ ���� �� ȿ����
    // position�� ���� ������ ������Ʈ���� rigidbody�� ���� �ʴ°� ����
    // (�ݶ��̴����� rigidbody�� ������� ��ġ�� �����ؾ��ϴ� ��찡 �߻���)

    // Rigidbody �� ���� ������Ʈ�� ���� ������ �����ؼ� �浹, ��, �߷� ����
    // �ɸ����� ��ȣ�ۿ��� �����ϰ� ���ִ� ������Ʈ

    // Rigidbody.AddForce(Vector3 dir, ForceMode mode);
    // �������� ������ ���� �������� �����ϰ�, ���� �ִ� ������ ���� ���������� ó������
    // �������� ���� �������� ó���� �� ����

}