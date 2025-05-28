using UnityEngine;

public enum Type
{
    노멀, 불, 물, 풀, 전기, 독, 고스트
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

