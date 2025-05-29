using UnityEngine;

[CreateAssetMenu(menuName = "Tower")]
public class TowerData : ScriptableObject
{
    public Sprite icon;           //Ÿ�� �̹���
    public string towerName;      //Ÿ�� �̸�
    public float damage;          //������
    public float range;           //���� ����
    public float attackSpeed;     //���� �ֱ�
    public int cost;              //����
    public GameObject prefab;     //Ÿ�� ������Ʈ
    public float weight = 1.0f;
}