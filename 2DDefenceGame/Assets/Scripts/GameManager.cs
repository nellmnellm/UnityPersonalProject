using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public int waveCount = 0;
    public int gold = 100;

    public void Earn(int amount)
    {
        gold += amount;
        UIManager.instance.UpdateGoldUI(gold);
    }

    public bool Cost(int amount)
    {
        if (gold < amount)
            return false;
        gold -= amount;
        UIManager.instance.UpdateGoldUI(gold);
        return true;

    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }
}
