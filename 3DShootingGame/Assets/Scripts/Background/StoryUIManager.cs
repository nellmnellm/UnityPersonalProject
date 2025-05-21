using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryUIManager : MonoBehaviour
{
    public static StoryUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public GameObject storyPanel;
    public Image storyImage;
    public Sprite[] storySprites;
    private int currentIndex = 0;

    private void Start()
    {
        storyPanel.SetActive(false);
    }

    public void ShowStory()
    {
        currentIndex = 0;
        storyPanel.SetActive(true);
        storyImage.sprite = storySprites[currentIndex];
    }

    public void OnClickNext()
    {
        currentIndex++;

        if (currentIndex >= storySprites.Length)
        {
            // 마지막 이미지였으면 현재 씬 이름 확인 후 다음 씬 결정
            string currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                case "Stage 1":
                    SceneManager.LoadScene("Stage 2");
                    break;
                case "Stage 2":
                    SceneManager.LoadScene("Stage 3");
                    break;
                case "Stage 3":
                    SceneManager.LoadScene("Title"); // 예: 엔딩 후 타이틀
                    break;
                default:
                    Debug.LogWarning("Unknown scene name: " + currentScene);
                    break;
            }
        }
        else
        {
            storyImage.sprite = storySprites[currentIndex];
        }
    }
}