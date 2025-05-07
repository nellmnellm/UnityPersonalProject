using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverText; // TMP 오브젝트
    [SerializeField] private GameObject goToTitleButton; // 버튼 오브젝트

    [SerializeField] private TextMeshProUGUI scoreText; // score
    private Transform playerTransform; // 플레이어위치.
    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    

    private void Start()
    {
        // 씬 시작 시 플레이어 위치 기록
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }
    private void Update()
    {
        if (playerTransform != null && scoreText != null)
        {
            float distance = playerTransform.position.z;
            int score = Mathf.FloorToInt(distance * 10); // 10배 확대
            scoreText.text = $"Score{score}";
        }
    }
    public void GameOver()
    {
        gameOverText.SetActive(true);
        goToTitleButton.SetActive(true);
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene"); 
    }

    public void InitGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            
        }

        if (scoreText != null)
        {
            scoreText.text = "Score\n0";
        }

        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (goToTitleButton != null)
            goToTitleButton.SetActive(false);


    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene") // ← 게임 씬 이름
        {
            InitGame();
        }
    }

}
