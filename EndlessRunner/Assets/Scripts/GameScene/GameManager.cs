using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverText; // TMP ������Ʈ
    [SerializeField] private GameObject goToTitleButton;
    private Transform playerTransform; // �÷��̾���ġ.
    private void Awake()
    {
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // �� ��ȯ���� ����
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
        // �� ���� �� �÷��̾� ��ġ ���
        InitGame();
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

        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (goToTitleButton != null)
            goToTitleButton.SetActive(false);


    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene") // �� ���� �� �̸�
        {
            InitGame();
        }
    }

}
