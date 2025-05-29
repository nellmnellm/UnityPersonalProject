using UnityEngine;

[CreateAssetMenu(menuName = "Tower")]
public class TowerData : ScriptableObject
{
    public Sprite icon;           //타워 이미지
    public string towerName;      //타워 이름
    public float damage;          //데미지
    public float range;           //공격 범위
    public float attackSpeed;     //공격 주기
    public int cost;              //가격
    public GameObject prefab;     //타워 오브젝트
    public float weight = 1.0f;
}