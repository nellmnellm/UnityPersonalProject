using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        // "GameScene"�� ��ȯ�� �� �̸�
        SceneManager.LoadScene("GameScene");
    }
}