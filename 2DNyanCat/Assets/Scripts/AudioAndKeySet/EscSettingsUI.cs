using UnityEngine;
using UnityEngine.UI;

public class EscSettingsUI : MonoBehaviour
{
    [Header("전체 메뉴")]
    public GameObject settingsMenu;

    [Header("탭 패널")]
    public GameObject soundPanel;
    public GameObject keyPanel;
    public Button soundTabButton;
    public Button keyTabButton;

    [Header("사운드 슬라이더")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider inGameSlider;

    //버튼의 색깔 조절
    private Color selectedColor = Color.white;
    private Color unselectedColor = new Color(0.5f, 0.5f, 0.5f);


    void Awake()
    {
        settingsMenu.SetActive(false);
    }


    void Start()
    {
        masterSlider.value = AudioManager.Instance.masterVolume;
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        inGameSlider.value = AudioManager.Instance.inGameVolume;

        masterSlider.onValueChanged.AddListener(value =>
           AudioManager.Instance.SetVolume("Master", value));
        bgmSlider.onValueChanged.AddListener(value =>
          AudioManager.Instance.SetVolume("BGM", value));
        sfxSlider.onValueChanged.AddListener(value =>
          AudioManager.Instance.SetVolume("SFX", value));
        inGameSlider.onValueChanged.AddListener(value =>
          AudioManager.Instance.SetVolume("InGame", value));

        ShowSoundSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingsMenu.SetActive(!settingsMenu.activeSelf);
        }
    }

    public void ShowSoundSettings()
    {
        soundPanel.SetActive(true);
        keyPanel.SetActive(false);

        soundTabButton.image.color = selectedColor;
        keyTabButton.image.color = unselectedColor;
    }

    public void ShowKeySettings()
    {
        soundPanel.SetActive(false);
        keyPanel.SetActive(true);

        soundTabButton.image.color = unselectedColor;
        keyTabButton.image.color = selectedColor;
    }

   


}