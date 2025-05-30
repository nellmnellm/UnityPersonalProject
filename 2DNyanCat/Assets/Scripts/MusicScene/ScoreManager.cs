using System;
using UnityEngine;

public enum Judgement
{
    Perfect,
    Great,
    Good,
    Miss
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }
    public int CurrentCombo { get; private set; }
    public int MaxCombo { get; private set; }

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
        int baseScore = result switch
        {
            Judgement.Perfect => 1000,
            Judgement.Great => 700,
            Judgement.Good => 400,
            Judgement.Miss => 0,
            _ => 0
        };

        if (result != Judgement.Miss)
        {
            CurrentCombo++;
            float comboMultiplier = 1f + Mathf.Log(Mathf.Max(1, CurrentCombo), 16);
            int totalScore = Mathf.RoundToInt(baseScore * comboMultiplier);
            CurrentScore += totalScore;

            MaxCombo = Mathf.Max(MaxCombo, CurrentCombo);
            OnComboChanged?.Invoke();
        }
        else
        {
            CurrentCombo = 0;
            OnComboChanged?.Invoke();
        }

        OnScoreChanged?.Invoke();
    }
}

