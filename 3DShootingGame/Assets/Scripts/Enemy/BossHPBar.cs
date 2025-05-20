using UnityEngine;
using UnityEngine.UI;

public interface IBoss
{
    float HPNormalized { get; }
    Sprite HPBarSprite { get; }
}
public class BossHPBar : MonoBehaviour
{
    public Slider hpSlider; // Slider 컴포넌트 참조
    public Image Image;
    private IBoss boss;

    public void SetBoss(IBoss bossTarget)
    {
        boss = bossTarget;

        if (boss != null)
        {
            hpSlider.gameObject.SetActive(true);
            Image.gameObject.SetActive(true);
            Image.sprite = boss.HPBarSprite;
        }
        else
        {
            hpSlider.gameObject.SetActive(false);
            Image.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (boss == null || (boss is MonoBehaviour mb && mb == null))
        {
            SetBoss(null); 
            return;
        }

        hpSlider.value = boss.HPNormalized; // 0~1로 적용
    }
}