using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RideDisplayUI : MonoBehaviour
{
    public Image rideDisplayImage; // Ÿ��Ʋ���� ������ �̹���
    public Sprite defaultRideSprite; // �⺻ �� �̹���
    public List<RideSpriteEntry> rideSprites; // �̸�-��������Ʈ ����

    [System.Serializable]
    public class RideSpriteEntry
    {
        public string rideName;
        public Sprite sprite;
    }

    void Start()
    {
        string selectedRide = PlayerPrefs.GetString("SelectedRide", "Horse");

        // ��Ī�� ��������Ʈ ã��
        Sprite rideSprite = GetSpriteForRide(selectedRide);
        rideDisplayImage.sprite = rideSprite;
    }

    public Sprite GetSpriteForRide(string rideName)
    {
        foreach (var entry in rideSprites)
        {
            if (entry.rideName == rideName)
                return entry.sprite;
        }
        return defaultRideSprite;
    }

    public void UpdateDisplay()
    {
        string selectedRide = PlayerPrefs.GetString("SelectedRide", "Horse");
        Sprite rideSprite = GetSpriteForRide(selectedRide);
        rideDisplayImage.sprite = rideSprite;
    }
}
