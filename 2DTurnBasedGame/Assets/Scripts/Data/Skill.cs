using UnityEngine;

[CreateAssetMenu (fileName = "SK", menuName = "Battle/Skill")]
public class Skill : ScriptableObject
{
    public Type skillType;
    public string skillName;
    public int damage;
    public string description;



}
