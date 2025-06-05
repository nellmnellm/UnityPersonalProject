using System;
using UnityEngine;

public enum Judgement
{
    Perfect,
    Great,
    Good,
    Miss
}
public enum ResultBadge
{
    None,
    FullCombo,
    AllPerfect
}
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }
    public int CurrentCombo { get; private set; }
    public int MaxCombo { get; private set; }
    public int PerfectCount { get; private set; }
    public int GreatCount { get; private set; }
    public int GoodCount { get; private set; }
    public int MissCount { get; private set; }

    public int TotalNotes { get; private set; } 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Action OnScoreChanged;
    public Action OnComboChanged;
    public Action OnPerfectChanged;
    public Action OnGreatChanged;
    public Action OnGoodChanged;
    public Action OnMissChanged;
    /*public void ResetScore()
    {
        CurrentScore = 0;
        CurrentCombo = 0;
        MaxCombo = 0;
    }*/

    public void Start()
    {
        CurrentScore = 0;
        CurrentCombo = 0;
        MaxCombo = 0;
    }
    public void RegisterJudgement(Judgement result)
    {
        int baseScore = 0;


        switch (result)
        {
            case Judgement.Perfect:
                {
                    PerfectCount++;
                    baseScore = 1000;
                    OnPerfectChanged?.Invoke();
                    break;
                }
            case Judgement.Great:
                {
                    GreatCount++;
                    baseScore = 700;
                    OnGreatChanged?.Invoke();
                    break;
                }
            case Judgement.Good:
                {
                    GoodCount++;
                    baseScore = 400;
                    OnGoodChanged?.Invoke();
                    break;
                }
            case Judgement.Miss:
                {
                    MissCount++;
                    baseScore = 0;
                    OnMissChanged?.Invoke();
                    break;
                }
        
        }
        if (result != Judgement.Miss)
        {
            
            CurrentCombo++;
            float comboMultiplier = 1f + Mathf.Log(Mathf.Max(1, CurrentCombo), 16);
            int totalScore = Mathf.RoundToInt(baseScore * SettingManager.Instance.playerSettings.ScoreMultiplier * comboMultiplier);
            CurrentScore += totalScore;

            MaxCombo = Mathf.Max(MaxCombo, CurrentCombo);
        }
        else
        {
            CurrentCombo = 0;
        }
        OnComboChanged?.Invoke();
        OnScoreChanged?.Invoke();
    }

    public void SetTotalNotes(int total)
    {
        TotalNotes = total;
    }

    public ResultBadge GetResultBadge()
    {
        if (MissCount == 0 && GoodCount == 0 && GreatCount == 0)
            return ResultBadge.AllPerfect;

        if (MissCount == 0)
            return ResultBadge.FullCombo;

        return ResultBadge.None;
    }

    public float GetAccuracyScore()
    {
        if (TotalNotes == 0) return 0f;

        float weightedHits =
            PerfectCount * 1f +
            GreatCount * 0.7f +
            GoodCount * 0.4f;

        return weightedHits * SettingManager.Instance.playerSettings.ScoreMultiplier / TotalNotes ;
    }
    public string GetRank()
    {
        float acc = GetAccuracyScore();
        if (acc >= 1.00f) return "S+";
        if (acc >= 0.90f) return "S";
        if (acc >= 0.80f) return "A";
        if (acc >= 0.70f) return "B";
        if (acc >= 0.60f) return "C";
        return "D";
    }
}

