using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerSelectionManager
{
    public static GameObject selectedPlayerPrefab;
}

public class StartGameButton : MonoBehaviour
{
    public CharacterCarouselManager carouselManager; // 에디터에서 Drag & Drop

    public void StartGame()
    {
        int selectedIndex = carouselManager.GetCurrentIndex(); // 선택된 캐릭터 인덱스
        PlayerSelectionManager.selectedPlayerPrefab = carouselManager.playerPrefabs[selectedIndex];

        SceneManager.LoadScene("Stage 1"); // 게임 씬으로 이동
    }
}