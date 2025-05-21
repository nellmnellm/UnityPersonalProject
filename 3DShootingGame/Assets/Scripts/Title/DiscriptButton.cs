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
            // 첫 번째 클릭: 텍스트 토글
            if (text1 != null) text1.gameObject.SetActive(false);
            if (text2 != null) text2.gameObject.SetActive(true);
        }
        else if (clickCount == 2)
        {
            if (text2 != null) text2.gameObject.SetActive(false);
            if (text1 != null) text1.gameObject.SetActive(true);
            // 두 번째 클릭: 이미지 전체 비활성화 (Start 화면 복귀용)
            if (imageRoot != null) imageRoot.gameObject.SetActive(false);
            clickCount = 0;
        }
    }
}