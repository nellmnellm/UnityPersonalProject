using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectUI : MonoBehaviour
{
    private PlayerSettingsData playerSettings;
    [Header("음표 속도 관련")]
    public Slider speedSlider;
    public TMP_Text speedText;
    public TMP_Text scoreMultiplierText; // 속도에 따른 배율 적용
    [Header("뮤비 밝기 관련")]
    public Slider mvAlphaSlider;
    public TMP_Text mvAlphaText;
    public Toggle mvToggle;

    [Header("노트 프리팹 관련")]
    public NoteSkinData[] allNoteSkins;
    public Image previewImageUI; // 프리뷰 보여줄 Image
    private int currentSkinIndex = 0;

    void Start()
    {
        playerSettings = SettingManager.Instance.playerSettings;

        if (playerSettings == null)
        {
            Debug.LogError("PlayerSettingsData가 연결되지 않았습니다.");
            return;
        }
        if (playerSettings.selectedNoteSkin == null)
        {
            playerSettings.selectedNoteSkin = allNoteSkins[0];
        }
        currentSkinIndex = System.Array.IndexOf(allNoteSkins, playerSettings.selectedNoteSkin);
        if (currentSkinIndex < 0) currentSkinIndex = 0;


        //슬라이더에 지금 값 적용.
        speedSlider.value = playerSettings.noteSpeed;
        mvAlphaSlider.value = playerSettings.MVBright;
        mvToggle.isOn = playerSettings.showMV;

        //UI를 갱신함
        OnSpeedChanged(playerSettings.noteSpeed);
        OnMVAlphaChanged(playerSettings.MVBright);
        OnMVOnoffChanged(playerSettings.showMV);
        UpdateNoteSkinPreview();
    }

    public void OnSpeedChanged(float value)
    {
        playerSettings.noteSpeed = value;
        speedText.text = $"{(value / 2).ToString()}";

        float multiplier = 0.8f + playerSettings.noteSpeed * 0.05f;
        scoreMultiplierText.text = $"×{multiplier:0.00}";
    }
    public void OnMVAlphaChanged(float value)
    {
        playerSettings.MVBright = value;
        mvAlphaText.text = $"{(int) (value * 100)}%";
    }
    public void OnMVOnoffChanged(bool value)
    {
        playerSettings.showMV = value;
    }

    void UpdateNoteSkinPreview()
    {
        var skin = allNoteSkins[currentSkinIndex];
        previewImageUI.sprite = skin.previewImage;
        playerSettings.selectedNoteSkin = skin;
    }

    public void OnClickPrevSkin()
    {
        currentSkinIndex = (currentSkinIndex - 1 + allNoteSkins.Length) % allNoteSkins.Length;
        UpdateNoteSkinPreview();
    }

    public void OnClickNextSkin()
    {
        currentSkinIndex = (currentSkinIndex + 1) % allNoteSkins.Length;
        UpdateNoteSkinPreview();
    }



}