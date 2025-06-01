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
            Debug.LogError("���õ� �� ������ �����ϴ�!");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� �����ϴ�.");
            return;
        }

        SetupVideoPlayer(); // GameManager�� �����ϰ� ����
        StartCoroutine(PrepareAndPlayVideo());
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
        videoRawImage = rawImage;
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
            Debug.LogError($"������ : ����� Ŭ�� �ε� ����: {audioKey}");
            yield break;
        }

        audioSource.clip = clip;
        dspStartTime = AudioSettings.dspTime + 0.1;

        audioSource.PlayScheduled(dspStartTime);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log($"������ DSP Ÿ�̹� ����: {dspStartTime}");
    }

    void Update()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared)
            return;

        // ���콺 Ŭ�� -> ��Ʈ ����
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
            Debug.Log($"��Ʈ �߰���: time={note.time}, height={note.height}");
        }

        // S Ű -> ����
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveNotesToJson();
        }
    }

    private void SaveNotesToJson()
    {
        string baseName = Path.GetFileNameWithoutExtension(SongLoader.SelectedSong.videoFileName);
        string fileName = baseName + ".json";
        //Charts ������ ä�� ����
        string chartDir = Path.Combine(Application.streamingAssetsPath, "Charts");
        Directory.CreateDirectory(chartDir); // ������ �ڵ� ����
        string path = Path.Combine(chartDir, fileName);
        if (File.Exists(path))
        {
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupPath = path.Replace(".json", $"_backup_{timestamp}.json");
            File.Copy(path, backupPath, overwrite: true);
            Debug.Log("���� ���� �����: " + backupPath);
        }

        NoteDataList wrapper = new NoteDataList { notes = notes };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);


        Debug.Log($"��Ʈ�� ����Ǿ����ϴ�: {path}");
    }
}