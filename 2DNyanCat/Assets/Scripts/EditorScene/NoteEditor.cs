using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class NoteEditor : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private Camera mainCamera;

    private double realStartTime = 0;
    private Vector2 videoSize = new Vector2(1920, 1080);
    private List<NoteData> notes = new List<NoteData>();

    public InputBindingsData bindingsData;

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

        SetupVideoPlayer(0.3f); // GameManager�� �����ϰ� ����
        StartCoroutine(PrepareAndPlayVideo());
    }

    private void OnEnable()
    {
        bindingsData.save.action.Enable(); // Action Map �̸��� Gameplay�� ���
        bindingsData.save1.action.Enable();
        bindingsData.save.action.performed += OnSave;
        bindingsData.save1.action.performed += OnSave;

    }

    private void OnDisable()
    {
        bindingsData.save.action.performed -= OnSave;
        bindingsData.save1.action.performed -= OnSave;
        bindingsData.save.action.Disable(); 
        bindingsData.save1.action.Disable();
    }
    private void SetupVideoPlayer(float mvalpha)
    {
        Camera.main.backgroundColor = new Color(0.7f, 0.7f, 0.9f);
        // 1. Canvas ����
        GameObject canvasGO = new GameObject("VideoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        //ĵ���� ������ ����
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Screen.width / Screen.height;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        canvas.sortingOrder = -10;

        // 2. RawImage ����
        GameObject rawImageGO = new GameObject("VideoDisplay", typeof(RawImage), typeof(RectTransform));
        rawImageGO.transform.SetParent(canvasGO.transform, false);
        RawImage rawImage = rawImageGO.GetComponent<RawImage>();
        rawImage.color = new Color(1f, 1f, 1f, mvalpha);
        RectTransform rt = rawImageGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
        rt.anchoredPosition = Vector2.zero;

        // 3. RenderTexture ����
        RenderTexture renderTexture = new RenderTexture((int)videoSize.x, (int)videoSize.y, 0);
        rawImage.texture = renderTexture;

        rawImage.enabled = SettingManager.Instance.playerSettings.showMV;
        // 4. VideoPlayer GameObject ����
        GameObject videoGO = new GameObject("VideoPlayer", typeof(VideoPlayer), typeof(AudioSource));
        videoPlayer = videoGO.GetComponent<VideoPlayer>();
        audioSource = videoGO.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = AudioManager.Instance.inGameMusicGroup;

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
            Debug.LogError($"����� Ŭ�� �ε� ����: {audioKey}");
            yield break;
        }

        audioSource.clip = clip;

        realStartTime = Time.realtimeSinceStartupAsDouble + 0.1;
        audioSource.PlayScheduled(AudioSettings.dspTime + 0.1);
        /*dspStartTime = AudioSettings.dspTime + 0.1; 
        audioSource.PlayScheduled(dspStartTime);*/
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log($"������ Ÿ�̹� ���� (RealTime): {realStartTime}");

    }

    private void OnSave(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        float currentTime = (float)(Time.realtimeSinceStartupAsDouble - realStartTime);
        float clampedHeight = Mathf.Clamp(worldPos.y, -4.2f, 4.2f);

        NoteData note = new NoteData
        {
            time = currentTime,
            height = clampedHeight
        };
        notes.Add(note);
        Debug.Log($"��Ʈ �߰���: time={note.time}, height={note.height}");
    }
    void Update()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared)
            return;

        // S Ű -> ����
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveNotesToJson();
        }

        if (Input.GetKeyDown (KeyCode.Escape))
        {
            SceneManager.LoadScene("SongSelectScene");
        }
    }

    private void SaveNotesToJson()
    {
        string baseName = Path.GetFileNameWithoutExtension(SongLoader.SelectedSong.videoFileName);
        string chartDir = Path.Combine(Application.streamingAssetsPath, "Charts");
        Directory.CreateDirectory(chartDir); // ���丮������ �ڵ� ����

        int index = 1;
        string fileName;
        string path;
        do
        {
            fileName = $"{baseName}_{index}.json";
            path = Path.Combine(chartDir, fileName);
            index++;
        }
        while (File.Exists(path));

        NoteDataList wrapper = new NoteDataList { notes = notes };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);


        Debug.Log($"��Ʈ�� ����Ǿ����ϴ�: {path}");
    }
}