using System.Collections;
using System.IO;
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

    //싱글톤
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
    
    //비디오 재생 관련
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private double dspStartTime = 0;
    private double realStartTime = 0;
    //private double videoStartTime = 0;
    private Vector2 videoSize = new(1920, 1080);
    private bool songEnded = false;
    //RawImage의 RayCaster를 꺼주기위해 필드 선언
    private RawImage videoRawImage;
   // Result UI 관련 (오브젝트 연결)

   [Header ("Result UI")]
    public GameObject ResultUI;
    public Image titleImageUI;
    public TMP_Text songNameText;
    public TMP_Text difficultyText;
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text perfectText;
    public TMP_Text greatText;
    public TMP_Text goodText;
    public TMP_Text missText;
    public Image fadePanel;
    public TMP_Text informTMP;  //정보 글자 흐리게 해서 10초후 꺼지게.
    public GameObject castlePrefab;  // 뒤의 캐릭터나 성 오브젝트 온오프
    public GameObject deadLinePrefab; // 노트 데드라인 온오프
    //판정 오프셋 관련 필드
    [SerializeField] private TMP_Text judgementOffsetText;
    [SerializeField] private Transform playerTransform;

    private Coroutine offsetCoroutine;
    
    [SerializeField] private AudioClip resultBGMClip; //result창 열렸을때 싸운드
    private AudioSource resultAudioSource;

    public SceneEscSound escSound;// 나가는 사운드
    //스포너를 위한 프로퍼티 전달 


    public double RealtimeStartTime => realStartTime;

    public float SongTime
    {
        get
        {
            if (realStartTime <= 0)
                return 0f; // 아직 음악 시작 안됨

            return (float)(Time.realtimeSinceStartupAsDouble - RealtimeStartTime);
        }
    }
    public float SongLength => audioSource != null && audioSource.clip != null ? (float)audioSource.clip.length : 0.01f;
    public bool SongEnded => songEnded;
    //public double DSPStartTime => dspStartTime;
    public SongData CurrentSong { get; private set; }

    void Start()
    {
        StartCoroutine(FadeOutAfterDelay(5f, 2f));
        if (ResultUI != null)
            ResultUI.SetActive(false);

        if (SongLoader.SelectedSong == null)
        {
            Debug.LogError("선택된 곡이 없습니다. 이전 씬에서 SongLoader.SelectedSong 설정이 필요합니다.");
            SceneManager.LoadScene("SongSelectScene"); // 안전 처리
            return;
        }
        CurrentSong = SongLoader.SelectedSong;
        float MVAlphaValue = SettingManager.Instance.playerSettings.MVBright;
        SetupVideoPlayer(MVAlphaValue);
        videoPlayer.loopPointReached += OnSongEnded;
        StartCoroutine(PrepareAndPlayVideo());
        //result창 사운드.
        resultAudioSource = gameObject.AddComponent<AudioSource>();
        resultAudioSource.outputAudioMixerGroup = AudioManager.Instance.bgmGroup;
        resultAudioSource.loop = true;
        resultAudioSource.playOnAwake = false;
    }
    private IEnumerator FadeOutAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        Color originalColor = informTMP.color;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            informTMP.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        informTMP.gameObject.SetActive(false); // 완전히 숨기기
    }
    //Start에서 UI 설정 (나중에 리절트창에 띄우는 용도)
    private void SetupResultUI()
    {
        if (titleImageUI != null)
            titleImageUI.sprite = CurrentSong.titleImage;

        if (songNameText != null)
            songNameText.text = CurrentSong.songName;

        if (difficultyText != null)
            difficultyText.text = CurrentSong.difficulty;

        if (scoreText != null)
            scoreText.text = ScoreManager.Instance.CurrentScore.ToString();

        if (comboText != null)
            comboText.text = ScoreManager.Instance.MaxCombo.ToString();

        if (perfectText != null)
            perfectText.text = ScoreManager.Instance.PerfectCount.ToString();

        if (greatText != null)
            greatText.text = ScoreManager.Instance.GreatCount.ToString();

        if (goodText != null)
            goodText.text = ScoreManager.Instance.GoodCount.ToString();

        if (missText != null)
            missText.text = ScoreManager.Instance.MissCount.ToString();
    }
    //리절트창에서 곡 선택 씬으로 가는 용도 //버튼에도 맵핑 + Enter에도 맵핑
    public void GoToSongSelectScene()
    {
        StartCoroutine(FadeAndLoadScene("SongSelectScene"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("Fade Panel이 연결되지 않았습니다!");
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        escSound.PlayEscSound();

        float duration = 0.5f;
        float elapsed = 0f;

        Color startColor = fadePanel.color;
        Color endColor = new Color(0, 0, 0, 1);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadePanel.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }

        fadePanel.color = endColor;
        SceneManager.LoadScene(sceneName);
    }

    
    private IEnumerator PrepareAndPlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        string audioKey = $"Audio/{Path.GetFileNameWithoutExtension(CurrentSong.audioFileName)}";
        AudioClip clip = Resources.Load<AudioClip>(audioKey);
        if (clip == null)
        {
            Debug.LogError($"오디오 클립 로드 실패: {audioKey}");
            yield break;
        }

        audioSource.clip = clip;
       
        realStartTime = Time.realtimeSinceStartupAsDouble + 0.1;
        audioSource.PlayScheduled(AudioSettings.dspTime + 0.1);
        /*dspStartTime = AudioSettings.dspTime + 0.1; 
        audioSource.PlayScheduled(dspStartTime);*/
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0.1f);
        //Debug.Log($"[SYNC] dspStartTime: {dspStartTime}, current dsp: {AudioSettings.dspTime}");

    }

    private void OnSongEnded(VideoPlayer vp)
    {
        if (vp.time < 1f) return;
        Debug.Log("Song Finished!");
        songEnded = true;
        ResultUI.SetActive(true);
        if (videoRawImage != null)
            videoRawImage.raycastTarget = false;
        SetupResultUI();
        PlayResultBGM(); // 
    }
    private void PlayResultBGM()
    {
        if (resultBGMClip == null)
        {
            Debug.LogWarning("Result BGM Clip not assigned.");
            return;
        }

        resultAudioSource.clip = resultBGMClip;
        resultAudioSource.Play();
    }

    private void SetupVideoPlayer(float mvalpha)
    {
        Camera.main.backgroundColor = new Color(0.7f, 0.7f, 0.9f);
        // 1. Canvas 생성
        GameObject canvasGO = new GameObject("VideoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        //캔버스 사이즈 조절
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Screen.width / Screen.height;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        canvas.sortingOrder = -10;

        // 2. RawImage 생성
        GameObject rawImageGO = new GameObject("VideoDisplay", typeof(RawImage), typeof(RectTransform));
        rawImageGO.transform.SetParent(canvasGO.transform, false);
        RawImage rawImage = rawImageGO.GetComponent<RawImage>();
        rawImage.color = new Color(1f, 1f, 1f, mvalpha);
        videoRawImage = rawImage;
        RectTransform rt = rawImageGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height); 
        rt.anchoredPosition = Vector2.zero;

        // 3. RenderTexture 생성
        RenderTexture renderTexture = new RenderTexture((int)videoSize.x, (int)videoSize.y, 0);
        rawImage.texture = renderTexture;

        rawImage.enabled = SettingManager.Instance.playerSettings.showMV;
        // 4. VideoPlayer GameObject 생성
        GameObject videoGO = new GameObject("VideoPlayer", typeof(VideoPlayer), typeof(AudioSource));
        videoPlayer = videoGO.GetComponent<VideoPlayer>();
        audioSource = videoGO.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = AudioManager.Instance.inGameMusicGroup;

        // 5. VideoPlayer 설정
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.skipOnDrop = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, CurrentSong.videoFileName);

        videoPlayer.controlledAudioTrackCount = 0;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
    }

    private void Update()
    {
        Debug.Log($"[SongTime] DSP-based: {SongTime:F3}");
        Debug.Log($"{AudioSettings.dspTime}");
        if (ResultUI != null && ResultUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                escSound.PlayEscSound();
                GoToSongSelectScene();
            }
        }
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FadeAndLoadScene("SongSelectScene"));
        }
        
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ToggleMV();
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            ToggleCastle();
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            ToggleNoteDeadLine();
        }
    }

    private void ToggleCastle()
    {

        castlePrefab.SetActive(!castlePrefab.activeSelf);
    }

    private void ToggleNoteDeadLine()
    {
        deadLinePrefab.SetActive(!deadLinePrefab.activeSelf);
    }
    private void ToggleMV()
    {
        var settings = SettingManager.Instance.playerSettings;

        settings.showMV = !settings.showMV;
        videoRawImage.enabled = settings.showMV;
    }

    public void ShowJudgementOffset(float offset)
    {
        if (judgementOffsetText == null || playerTransform == null)
            return;

        if (offsetCoroutine != null)
            StopCoroutine(offsetCoroutine);

        offsetCoroutine = StartCoroutine(ShowOffsetCoroutine(offset));
    }

    private IEnumerator ShowOffsetCoroutine(float offset)
    {
        RectTransform rt = judgementOffsetText.GetComponent<RectTransform>();
        Vector3 playerPos = playerTransform.position;

        float xOffset = offset < 0 ? -5f : 3f;
        rt.position = new Vector3(playerPos.x + xOffset, playerPos.y, playerPos.z);

        string sign = offset < 0 ? "+" : "-";
        judgementOffsetText.text = offset < 0
            ? $"Fast\n{sign}{Mathf.Abs(offset * 1000f):0.0} ms"
            : $"Slow\n{sign}{Mathf.Abs(offset * 1000f):0.0} ms";

        judgementOffsetText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        judgementOffsetText.gameObject.SetActive(false);
    }

}