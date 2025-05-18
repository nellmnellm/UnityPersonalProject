using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text currentScoreUI;
    public TMP_Text highScoreUI;
    public int currentScore = 0;
    public int highScore;

    public int Score
    {
        get 
        {
            currentScoreUI.text = $"Score\n{currentScore}";
            return currentScore; 
        }
        set 
        { 
            currentScore = value;
            currentScoreUI.text = $"Score\n{currentScore}";

            if (currentScore > highScore)
            {
                highScore = currentScore;
                highScoreUI.text = $"Best\n{highScore}";

                PlayerPrefs.SetInt("HIGH_SCORE", highScore);
                PlayerPrefs.Save();
            }
        }
    }
    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HIGH_SCORE", 0);
        highScoreUI.text = $"Best\n{highScore}";
    }
    #region singleton
    public static ScoreManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
}
