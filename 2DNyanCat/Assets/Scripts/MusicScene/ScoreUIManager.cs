using System.Collections;
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
    public TMP_Text totalNoteText;
    public TMP_Text accuracyText;
    public TMP_Text rankText;
    public TMP_Text specialText;

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
        totalNoteText.text = ScoreManager.Instance.TotalNotes.ToString();
        accuracyText.text = $"{ScoreManager.Instance.GetAccuracyScore() * 100f:0.0}%";
        rankText.text = ScoreManager.Instance.GetRank();

        var badge = ScoreManager.Instance.GetResultBadge();

        switch (badge)
        {
            case ResultBadge.AllPerfect:
                specialText.text = "ALL PERFECT";
                break;
            case ResultBadge.FullCombo:
                specialText.text = "FULL COMBO";
                break;
            default:
                specialText.text = "";
                break;
        }
        if (badge != ResultBadge.None)
            StartCoroutine(AnimateColor(specialText));
    }
    IEnumerator AnimateColor(TMP_Text targetText)
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime;
            Color c = Color.Lerp(Color.cyan, Color.yellow, Mathf.PingPong(t, 1f));
            targetText.color = c;
            yield return null;
        }
    }
    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateUI;
        }
    }
}