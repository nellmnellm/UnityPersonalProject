using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /*private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }*/
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
        if (currentScoreUI != null)
        {
            currentScoreUI.text = $"Score\n{currentScore}";
        }
            highScore = PlayerPrefs.GetInt("HIGH_SCORE", 0);
        if (highScoreUI != null)
        {
            highScoreUI.text = $"Best\n{highScore}";
        }
    }
    
}
