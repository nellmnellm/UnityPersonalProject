using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    
    
    
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0);

        // transform.Translate(dir * speed * Time.deltaTime);

        

        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += dir * 2 * Time.deltaTime;
           
        }
        else
        {
            transform.position += dir * speed * Time.deltaTime;
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
}