using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RideDisplayUI : MonoBehaviour
{
    public Image rideDisplayImage; // 타이틀에서 보여줄 이미지
    public Sprite defaultRideSprite; // 기본 말 이미지
    public List<RideSpriteEntry> rideSprites; // 이름-스프라이트 매핑

    [System.Serializable]
    public class RideSpriteEntry
    {
        public string rideName;
        public Sprite sprite;
    }

    void Start()
    {
        string selectedRide = PlayerPrefs.GetString("SelectedRide", "Horse");

        // 매칭된 스프라이트 찾기
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
