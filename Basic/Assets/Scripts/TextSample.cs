using UnityEngine;
using UnityEngine.UI; //Text ����� ���� �߰�.
using TMPro; // textMeshPro ����� ���� �߰�.

public class TextSample : MonoBehaviour
{

    public TextMeshProUGUI tmp;
    public Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmp.text = "����������";
        text.text = "�ϳ�����";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
