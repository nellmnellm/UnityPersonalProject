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
            // 마지막 이미지였으면 씬 전환
            SceneManager.LoadScene("Stage 2");
        }
        else
        {
            storyImage.sprite = storySprites[currentIndex];
        }
    }
}