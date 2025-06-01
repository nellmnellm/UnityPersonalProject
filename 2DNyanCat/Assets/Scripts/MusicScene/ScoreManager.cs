using System;
using UnityEditor.Experimental.GraphView;
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
    public int PerfectCount { get; private set; }
    public int GreatCount { get; private set; }
    public int GoodCount { get; private set; }
    public int MissCount { get; private set; }

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

