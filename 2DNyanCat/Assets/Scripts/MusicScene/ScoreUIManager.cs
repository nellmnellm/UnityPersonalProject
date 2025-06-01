using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text perfectText;
    public TMP_Text greatText;
    public TMP_Text goodText;
    public TMP_Text missText;

    private void Start()
    {
        UpdateUI();

        ScoreManager.Instance.OnScoreChanged += UpdateUI;
        ScoreManager.Instance.OnComboChanged += UpdateUI;
        ScoreManager.Instance.OnPerfectChanged += UpdateUI;
        ScoreManager.Instance.OnGreatChanged += UpdateUI;
        ScoreManager.Instance.OnGoodChanged += UpdateUI;
        ScoreManager.Instance.OnMissChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score\n{ScoreManager.Instance.CurrentScore}";

        int combo = ScoreManager.Instance.CurrentCombo;
        comboText.text = combo > 0 ? $"Combo\n{combo}" : "";
        perfectText.text = $"{ScoreManager.Instance.PerfectCount}";
        greatText.text = $"{ScoreManager.Instance.GreatCount}";
        goodText.text = $"{ScoreManager.Instance.GoodCount}";
        missText.text = $"{ScoreManager.Instance.MissCount}";
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateUI;
        }
    }
}