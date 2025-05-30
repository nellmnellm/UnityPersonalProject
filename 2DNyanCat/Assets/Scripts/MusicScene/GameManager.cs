using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public static class SongLoader
{
    public static SongData SelectedSong;
}

public class GameManager : MonoBehaviour
{

    //�̱���
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    
    
    //���� ��� ����
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private double videoStartTime = 0;
    private Vector2 videoSize = new Vector2(1920, 1080);
    private bool songEnded = false;

    // Result UI ���� (������Ʈ ����)
    [Header ("Result UI")]
    public Image titleImageUI;
    public TMP_Text songNameText;
    public TMP_Text difficultyText;
    
    //�����ʸ� ���� ������Ƽ ���� 
    public float SongTime => videoPlayer != null && videoPlayer.isPrepared ? (float)(videoPlayer.time - videoStartTime) : 0f;
    public float SongLength => videoPlayer != null ? (float)videoPlayer.length : 0.01f;
    public bool SongEnded => songEnded;
    public SongData CurrentSong { get; private set; }
    //public string videoFileName;

    void Start()
    {
        
        if (SongLoader.SelectedSong == null)
        {
            Debug.LogError("���õ� ���� �����ϴ�. ���� ������ SongLoader.SelectedSong ������ �ʿ��մϴ�.");
            SceneManager.LoadScene("SongSelectScene"); // ���� ó��
            return;
        }
        CurrentSong = SongLoader.SelectedSong;
        SetupVideoPlayer();
        SetupSongUI();
        videoPlayer.loopPointReached += OnSongEnded;
        StartCoroutine(PrepareAndPlayVideo());
    }
    //Start���� UI ���� (���߿� ����Ʈâ�� ���� �뵵)
    private void SetupSongUI()
    {
        if (titleImageUI != null)
            titleImageUI.sprite = CurrentSong.titleImage;

        if (songNameText != null)
            songNameText.text = CurrentSong.songName;

        if (difficultyText != null)
            difficultyText.text = CurrentSong.difficulty;
    }

    private IEnumerator PrepareAndPlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        videoPlayer.GetTargetAudioSource(0).Play();
        yield return null; // 1������ ��ٷ��� ��� ����ȭ
        videoStartTime = videoPlayer.time; // �� ������ �������� SongTime ���
    }

    private void OnSongEnded(VideoPlayer vp)
    {
        if (vp.time < 1f) return;
        Debug.Log("Song Finished!");
        songEnded = true;
    }
    

    private void SetupVideoPlayer()
    {
        Camera.main.backgroundColor = new Color(0.7f, 0.7f, 0.9f);
        // 1. Canvas ����
        GameObject canvasGO = new GameObject("VideoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 2. RawImage ����
        GameObject rawImageGO = new GameObject("VideoDisplay", typeof(RawImage), typeof(RectTransform));
        rawImageGO.transform.SetParent(canvasGO.transform, false);
        RawImage rawImage = rawImageGO.GetComponent<RawImage>();
        rawImage.color = new Color(1f, 1f, 1f, 0.3f);
        RectTransform rt = rawImageGO.GetComponent<RectTransform>();
        rt.sizeDelta = videoSize;
        rt.anchoredPosition = Vector2.zero;

        // 3. RenderTexture ����
        RenderTexture renderTexture = new RenderTexture((int)videoSize.x, (int)videoSize.y, 0);
        rawImage.texture = renderTexture;

        // 4. VideoPlayer GameObject ����
        GameObject videoGO = new GameObject("VideoPlayer", typeof(VideoPlayer), typeof(AudioSource));
        videoPlayer = videoGO.GetComponent<VideoPlayer>();
        audioSource = videoGO.GetComponent<AudioSource>();

        // 5. VideoPlayer ����
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, CurrentSong.videoFileName);

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        videoPlayer.isLooping = false;


    }

    private void Update()
    {
        Debug.Log($"Video actual time: {videoPlayer.time}, offset: {videoStartTime}, songTime: {SongTime}");
    }
    //public SongData SelectedSong;     //������ �뷡.
}