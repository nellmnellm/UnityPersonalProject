using UnityEngine;

public class SoTester : MonoBehaviour
{
    public Item[] items;

    private void Start()
    {
        foreach (var item in items)
        {
            Debug.Log($"아이템 이름 => {item.name} : {item.description}\n가격 : {item.value}");
        }
    }
}