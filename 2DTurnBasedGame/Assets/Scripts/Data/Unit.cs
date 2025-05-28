using UnityEngine;

public enum Type
{
    ���, ��, ��, Ǯ, ����, ��, ��Ʈ
}
[CreateAssetMenu(fileName = "UN", menuName = "Battle/Unit")]
public class Unit : ScriptableObject
{
    public string unitName;
    public int maxHP;
    public int currentHP;

    public Type Type1;
    public Type Type2;
    
    public Skill[] skills;



}

