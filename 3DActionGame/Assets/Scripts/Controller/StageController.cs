using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class StageController : MonoBehaviour
{
    public static StageController Instance;

    
    public int StagePoint = 0;
    public TMP_Text pointText;

    private void Awake()
    {
        Instance = this;   
    }

    public void AddPoint(int score)
    {
        StagePoint += score;
        pointText.text = $"{StagePoint}";
    }

    
    public void FinishGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
