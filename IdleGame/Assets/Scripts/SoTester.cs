using UnityEngine;

public class SoTester : MonoBehaviour
{
    public Item[] items;

    private void Start()
    {
        foreach (var item in items)
        {
            Debug.Log($"������ �̸� => {item.name} : {item.description}\n���� : {item.value}");
        }
    }
}