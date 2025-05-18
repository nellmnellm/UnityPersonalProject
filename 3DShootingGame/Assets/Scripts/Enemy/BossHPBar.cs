using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Slider hpSlider; // Slider 컴포넌트 참조
    public Image Image;
    private Boss1 boss;

    public void SetBoss(Boss1 bossTarget)
    {
        boss = bossTarget;
        hpSlider.gameObject.SetActive(true);
        Image.gameObject.SetActive(true);
    }

    void Update()
    {
        if (boss == null || !boss.isActiveAndEnabled)
        {
            hpSlider.gameObject.SetActive(false); 
            Image.gameObject.SetActive(false);
            return;
        }

        hpSlider.value = boss.HPNormalized; // 0~1로 적용
    }
}