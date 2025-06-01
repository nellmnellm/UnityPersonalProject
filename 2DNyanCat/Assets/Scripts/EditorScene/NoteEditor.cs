using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NoteEditor : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private double dspStartTime = 0;
    private Vector2 videoSize = new Vector2(1920, 1080);
    private Camera mainCamera;
    private RawImage videoRawImage;

    private List<NoteData> notes = new List<NoteData>();

    void Start()
    {
        if (SongLoader.SelectedSong == null)
        {
            Debug.LogError("선택된 곡 정보가 없습니다!");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera가 없습니다.");
            return;
        }

        SetupVideoPlayer(); // GameManager와 동일하게 구성
        StartCoroutine(PrepareAndPlayVideo());
    }

    private void SetupVideoPlayer()
    {
        Camera.main.backgroundColor = new Color(0.7f, 0.7f, 0.9f);
        // 1. Canvas 생성
        GameObject canvasGO = new GameObject("VideoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 2. RawImage 생성
        GameObject rawImageGO = new GameObject("VideoDisplay", typeof(RawImage), typeof(RectTransform));
        rawImageGO.transform.SetParent(canvasGO.transform, false);
        RawImage rawImage = rawImageGO.GetComponent<RawImage>();
        rawImage.color = new Color(1f, 1f, 1f, 0.3f);
        videoRawImage = rawImage;
        RectTransform rt = rawImageGO.GetComponent<RectTransform>();
        rt.sizeDelta = videoSize;
        rt.anchoredPosition = Vector2.zero;

        // 3. RenderTexture 생성
        RenderTexture renderTexture = new RenderTexture((int)videoSize.x, (int)videoSize.y, 0);
        rawImage.texture = renderTexture;

        // 4. VideoPlayer GameObject 생성
        GameObject videoGO = new GameObject("VideoPlayer", typeof(VideoPlayer), typeof(AudioSource));
        videoPlayer = videoGO.GetComponent<VideoPlayer>();
        audioSource = videoGO.GetComponent<AudioSource>();

        // 5. VideoPlayer 설정
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.skipOnDrop = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, SongLoader.SelectedSong.videoFileName);

        videoPlayer.controlledAudioTrackCount = 0;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
    }

    private IEnumerator PrepareAndPlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        string audioKey = $"Audio/{Path.GetFileNameWithoutExtension(SongLoader.SelectedSong.audioFileName)}";
        AudioClip clip = Resources.Load<AudioClip>(audioKey);
        if (clip == null)
        {
            Debug.LogError($"에디터 : 오디오 클립 로드 실패: {audioKey}");
            yield break;
        }

        audioSource.clip = clip;
        dspStartTime = AudioSettings.dspTime + 0.1;

        audioSource.PlayScheduled(dspStartTime);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log($"에디터 DSP 타이밍 시작: {dspStartTime}");
    }

    void Update()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared)
            return;

        // 마우스 클릭 -> 노트 생성
        if (Input.GetMouseButtonDown(0) || 
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKeyDown(KeyCode.F)) 
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

            float currentTime = (float)(AudioSettings.dspTime - dspStartTime);

            float clampedHeight = Mathf.Clamp(worldPos.y, -4.2f, 4.2f);

            NoteData note = new NoteData
            {
                time = currentTime,
                height = clampedHeight
            };

            notes.Add(note);
            Debug.Log($"노트 추가됨: time={note.time}, height={note.height}");
        }

        // S 키 -> 저장
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveNotesToJson();
        }
    }

    private void SaveNotesToJson()
    {
        string baseName = Path.GetFileNameWithoutExtension(SongLoader.SelectedSong.videoFileName);
        string fileName = baseName + ".json";
        //Charts 폴더에 채보 저장
        string chartDir = Path.Combine(Application.streamingAssetsPath, "Charts");
        Directory.CreateDirectory(chartDir); // 없으면 자동 생성
        string path = Path.Combine(chartDir, fileName);
        if (File.Exists(path))
        {
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupPath = path.Replace(".json", $"_backup_{timestamp}.json");
            File.Copy(path, backupPath, overwrite: true);
            Debug.Log("기존 파일 백업됨: " + backupPath);
        }

        NoteDataList wrapper = new NoteDataList { notes = notes };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);


        Debug.Log($"노트가 저장되었습니다: {path}");
    }
}