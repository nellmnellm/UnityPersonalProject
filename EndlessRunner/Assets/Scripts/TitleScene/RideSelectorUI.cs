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

            // �̹��� ���� ����
            SetButtonColor(ride.rideButton, unlocked ? Color.white : Color.gray);

            // ��ư Ȱ��ȭ ����
            ride.rideButton.interactable = unlocked;

            // ��ư Ŭ�� �̺�Ʈ ����
            string rideNameCopy = ride.rideName;
            ride.rideButton.onClick.AddListener(() => SelectRide(rideNameCopy));
        }
    }

    void SelectRide(string rideName)
    {
        PlayerPrefs.SetString("SelectedRide", rideName);
        PlayerPrefs.Save();
        Debug.Log("������ ���̵�: " + rideName);
        rideDisplayUI.UpdateDisplay();
        
}

    // ���̵��� �ر��� �� ȣ�� (��: ��í ���)
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
        cb.pressedColor = color * 0.9f;      // ������ �� ��¦ ��Ӱ�
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