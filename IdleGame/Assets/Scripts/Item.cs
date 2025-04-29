using UnityEngine;

//ScriptableObject: ��ũ��Ʈó�� ����ϴ� ������Ʈ.
//����Ƽ�� �������� ������ �� ����. ����Ƽ ����.����� ���ο� �����ִ� ��������.
[CreateAssetMenu(fileName = "item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public Sprite Item_Image;
    public string description;
    public int value;
    private int id;

    public int ID { get { return id; } }


}
