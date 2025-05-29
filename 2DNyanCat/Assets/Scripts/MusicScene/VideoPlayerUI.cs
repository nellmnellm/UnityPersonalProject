using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerUI : MonoBehaviour
{ 
    public string videoFileName = "NyanCatShort.mp4"; // Assets/StreamingAssets/mv.mp4
    public Vector2 videoSize = new Vector2(1920, 1080);

    private void Start()
    {
        Camera.main.backgroundColor = new Color(0.7f, 0.7f, 0.9f);                                                       
        // 1. Canvas 持失
        GameObject canvasGO = new GameObject("VideoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 2. RawImage 持失
        GameObject rawImageGO = new GameObject("VideoDisplay", typeof(RawImage), typeof(RectTransform));
        rawImageGO.transform.SetParent(canvasGO.transform, false);
        RawImage rawImage = rawImageGO.GetComponent<RawImage>();
        rawImage.color = new Color(1f, 1f, 1f, 0.3f);
        RectTransform rt = rawImageGO.GetComponent<RectTransform>();
        rt.sizeDelta = videoSize;
        rt.anchoredPosition = Vector2.zero;

        // 3. RenderTexture 持失
        RenderTexture renderTexture = new RenderTexture((int)videoSize.x, (int)videoSize.y, 0);
        rawImage.texture = renderTexture;

        // 4. VideoPlayer GameObject 持失
        GameObject videoGO = new GameObject("VideoPlayer", typeof(VideoPlayer), typeof(AudioSource));
        VideoPlayer videoPlayer = videoGO.GetComponent<VideoPlayer>();
        AudioSource audioSource = videoGO.GetComponent<AudioSource>();

        // 5. VideoPlayer 竺舛
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.playOnAwake = true;
        audioSource.playOnAwake = true;

        videoPlayer.isLooping = true;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (vp) => {
            vp.Play();
            audioSource.Play();
        };
    }
}