using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        // "GameScene"은 전환할 씬 이름
        SceneManager.LoadScene("GameScene");
    }
}