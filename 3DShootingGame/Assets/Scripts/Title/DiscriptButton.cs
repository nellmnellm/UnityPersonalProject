using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiscriptButton : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;

    private int clickCount = 0;
    private Image imageRoot;

    private void Start()
    {
        imageRoot = GetComponent<Image>();

        if (text1 != null) text1.gameObject.SetActive(true);
        if (text2 != null) text2.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        clickCount++;

        if (clickCount == 1)
        {
            // ù ��° Ŭ��: �ؽ�Ʈ ���
            if (text1 != null) text1.gameObject.SetActive(false);
            if (text2 != null) text2.gameObject.SetActive(true);
        }
        else if (clickCount == 2)
        {
            if (text2 != null) text2.gameObject.SetActive(false);
            if (text1 != null) text1.gameObject.SetActive(true);
            // �� ��° Ŭ��: �̹��� ��ü ��Ȱ��ȭ (Start ȭ�� ���Ϳ�)
            if (imageRoot != null) imageRoot.gameObject.SetActive(false);
            clickCount = 0;
        }
    }
}