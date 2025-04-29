using UnityEngine;

//ScriptableObject: 스크립트처럼 사용하는 오브젝트.
//유니티에 에셋으로 저장할 수 있음. 유니티 실행.종료시 내부에 남아있는 데이터임.
[CreateAssetMenu(fileName = "item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public Sprite Item_Image;
    public string description;
    public int value;
    private int id;

    public int ID { get { return id; } }


}
