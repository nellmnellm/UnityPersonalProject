using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;

    private void Start()
    {
        UpdateScoreUI();
        UpdateComboUI();

        ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
        ScoreManager.Instance.OnComboChanged += UpdateComboUI;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = $"Score\n{ScoreManager.Instance.CurrentScore}";
    }

    private void UpdateComboUI()
    {
        int combo = ScoreManager.Instance.CurrentCombo;
        comboText.text = combo > 0 ? $"Combo\n{combo}" : "";
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreUI;
            ScoreManager.Instance.OnComboChanged -= UpdateComboUI;
        }
    }
}