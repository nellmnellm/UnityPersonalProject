using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text wave_text;
    public TMP_Text gold_text;

    public static UIManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateGoldUI(int gold)
    {
        gold_text.text = gold.ToString();
    }

    public void OnTowerBuildButtonClick()
    {
        Vector2 spawn = BuildPosition();
        TowerManager.instance.RandSpawnTower(spawn);
    }

    Vector2 BuildPosition()
    {
        return new Vector2(0, 0);
    }
}