using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class GachaSystem : MonoBehaviour
{
    public ParticleSystem summonEffect;
    public TMP_Text resultText;
    

    public RideSelectorUI rideSelectorUI; // 기존 UI와 연결
    public RideDisplayUI rideDisplayUI;

    public Image gacharesultImage;

    // 버튼 누르면 호출되는 메서드
    public void RollGacha()
    {
        StartCoroutine(GachaRoutine());
        resultText.text = "두구두구두구두구두구";
    }

    IEnumerator GachaRoutine()
    {
        gacharesultImage.sprite = null;
        // 이펙트 실행
        if (summonEffect != null)
            summonEffect.Play();

        // 연출 시간
        yield return new WaitForSeconds(2f);



        // 확률 기반 추첨
        string result = RollResult();

        
        // 매칭된 스프라이트 찾기
        Sprite rideSprite = rideDisplayUI.GetSpriteForRide(result);
        gacharesultImage.sprite = rideSprite;


        // 결과 표시
        resultText.text = "획득: " + result;
        
        // 해금 처리
        rideSelectorUI.UnlockRide(result);

        // UI 업데이트 반영 (선택적으로 다시 Start() 재실행하거나 별도 메서드 구성)
        rideSelectorUI.UpdateUI();
    }

    string RollResult()
    {
        float rand = Random.Range(0f, 100f);
        if (rand < 1f)
        {
            return "Tiger";
        }// 1%
        else if (rand < 20f)
        {
            return "Chicken";
        }// 19%
        else
        {

            return "Horse";
        }// 80%

        
    }
}