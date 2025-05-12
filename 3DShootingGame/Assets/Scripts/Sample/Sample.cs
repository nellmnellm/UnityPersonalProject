using System;
using UnityEngine;

[Serializable] //Ŭ������ ����ü�� �ν����Ϳ� ǥ���մϴ�.
public class Data
{
    [Range(1, 5)] public int value;    // ������ �����մϴ�. (�����̴� �������� ǥ��)
    [Multiline(5)] public string s;   // ���ڿ� �ۼ��� ���� ���� ���������ݴϴ�.
    [TextArea(3, 5)] public string s2; //���ڿ� �ۼ��� �ּ� ���ΰ� �ִ� ������ �����մϴ�.

    [SerializeField] float f; //�ʵ带 �ν����Ϳ� ǥ���մϴ�.

    [Tooltip("���� ������Ʈ")] public GameObject gameObject; //������ ���콺�� �����ٴ�� ������ ��.
}
public class NotSerial
{
    [Range(1, 5)] public int value;    // ������ �����մϴ�. (�����̴� �������� ǥ��)
    [Multiline(5)] public string s;   // ���ڿ� �ۼ��� ���� ���� ���������ݴϴ�.
    [TextArea(3, 5)] public string s2; //���ڿ� �ۼ��� �ּ� ���ΰ� �ִ� ������ �����մϴ�.
    [Multiline(3)][ContextMenuItem("SetStory", "setStroy")] public string s3;
    [SerializeField] float f; //�ʵ带 �ν����Ϳ� ǥ���մϴ�.
    [Space(10)] public bool ischeck; //���� ���ڸ�ŭ ������ ���� �� �ʵ� ǥ��
    [Tooltip("���� ������Ʈ")] public GameObject gameObject; //������ ���콺�� �����ٴ�� ������ ��.
}


//�÷��� �Ӽ��� �� enum�� ���� ������ �����մϴ�.
//ex) ī�޶��� CullingMask�� ���� Everything, None , ������ ���� ���� �� �� �ִµ�
//�� ����� ���ԵǼ� �����ϴ�.
//�÷��� ������ �� ������ ��Ʈ ������ ���ؼ� ĭ �̵��� �����մϴ�.
[Flags]
public enum TYPE
{
    �� = 1,
    Ǯ = 2,
    ���� = 4,
    ȭ�� = 8,
    ��Ʈ = 16,
    ������ = 32
}

//Add Component ��ư�� �̿��� �߰� ��� �������
//�޴��� Component ��ɿ� �߰��˴ϴ�.
[AddComponentMenu("Sample/Sample")]
public class Sample : MonoBehaviour
{
    public Data data;
    public NotSerial serial;

    public TYPE type;

    [Multiline(3)][ContextMenuItem("SetStory", "setStory")] public string s3;
    [Space(10)] public bool ischeck; //���� ���ڸ�ŭ ������ ���� �� �ʵ� ǥ��
    public void setStory()
    {
        s3 = "������ ������ ���ϰ� ���� ��ġ�ڽ��ϴ�.";
    }

    [ContextMenu("BoolCheck")]
    public void boolCheck()
    {
        if (ischeck)
        {
            ischeck = false;
        }
        else
        {
            ischeck = true;
        }
    }
}