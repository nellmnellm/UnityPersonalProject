using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class StoryPage
{
    public Sprite image;
    [TextArea(2, 5)]
    public string text;
}

public enum StoryType { Intro, Outro }
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
    public TMP_Text storyText;

    public StoryPage[] introPages;
    public StoryPage[] outroPages;

    private StoryPage[] currentPage;
    private int currentIndex = 0;
    private StoryType currentStoryType;

    private void Start()
    {
        storyPanel.SetActive(false);
    }

    private void Update()
    {
        if (storyPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            OnClickNext();
        }
    }
    public void ShowStory(StoryType type)
    {
        currentStoryType = type;
        currentPage = (type == StoryType.Intro) ? introPages : outroPages;
       
        if (currentPage == null || currentPage.Length == 0)
        {
            return;
        }
        currentIndex = 0;
        storyPanel.SetActive(true);
        UpdateStoryUI();

        if (type == StoryType.Intro)
        {
            Time.timeScale = 0f;
        }
    }
    private void UpdateStoryUI()
    {
        storyImage.sprite = currentPage[currentIndex].image;
        storyText.text = currentPage[currentIndex].text;
    }
    public void OnClickNext()
    {
        currentIndex++;

        if (currentIndex >= currentPage.Length)
        {
            storyPanel.SetActive(false);
            if (currentStoryType == StoryType.Intro)
            {
                //게임 시작
                Time.timeScale = 1f;
            }
            else
            {
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
                
        }
        else
        {
            UpdateStoryUI();
        }
    }
}