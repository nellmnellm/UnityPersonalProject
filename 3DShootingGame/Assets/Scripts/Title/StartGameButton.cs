using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerSelectionManager
{
    public static GameObject selectedPlayerPrefab;
}

public class StartGameButton : MonoBehaviour
{
    public CharacterCarouselManager carouselManager; // �����Ϳ��� Drag & Drop

    public void StartGame()
    {
        int selectedIndex = carouselManager.GetCurrentIndex(); // ���õ� ĳ���� �ε���
        PlayerSelectionManager.selectedPlayerPrefab = carouselManager.playerPrefabs[selectedIndex];

        SceneManager.LoadScene("Stage 1"); // ���� ������ �̵�
    }
}