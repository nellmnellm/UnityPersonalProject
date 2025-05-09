using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RideSelectorUI : MonoBehaviour
{

    

    [System.Serializable]
    public class RideUI
    {
        public string rideName;
        public Button rideButton;
    }

    public RideDisplayUI rideDisplayUI;

    public List<RideUI> rideOptionsUI;

    void Start()
    {
        foreach (var ride in rideOptionsUI)
        {
            bool unlocked = PlayerPrefs.GetInt("RideUnlocked_" + ride.rideName, ride.rideName == "Horse" ? 1 : 0) == 1;

            // 이미지 색상 설정
            SetButtonColor(ride.rideButton, unlocked ? Color.white : Color.gray);

            // 버튼 활성화 여부
            ride.rideButton.interactable = unlocked;

            // 버튼 클릭 이벤트 설정
            string rideNameCopy = ride.rideName;
            ride.rideButton.onClick.AddListener(() => SelectRide(rideNameCopy));
        }
    }

    void SelectRide(string rideName)
    {
        PlayerPrefs.SetString("SelectedRide", rideName);
        PlayerPrefs.Save();
        Debug.Log("선택한 라이딩: " + rideName);
        rideDisplayUI.UpdateDisplay();
        
}

    // 라이딩을 해금할 때 호출 (예: 가챠 결과)
    public void UnlockRide(string rideName)
    {
        PlayerPrefs.SetInt("RideUnlocked_" + rideName, 1);
        PlayerPrefs.Save();
    }

    void SetButtonColor(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color * 0.9f;      // 눌렀을 때 살짝 어둡게
        cb.selectedColor = color;
        cb.disabledColor = Color.gray * 0.7f;
        button.colors = cb;
    }
    public void UpdateUI()
    {
        foreach (var ride in rideOptionsUI)
        {
            bool unlocked = PlayerPrefs.GetInt("RideUnlocked_" + ride.rideName, ride.rideName == "Horse" ? 1 : 0) == 1;

            SetButtonColor(ride.rideButton, unlocked ? Color.white : Color.gray);
            ride.rideButton.interactable = unlocked;
        }
    }
}


//public string[] rideOptions = { "Horse", "Chicken", "Tiger" };