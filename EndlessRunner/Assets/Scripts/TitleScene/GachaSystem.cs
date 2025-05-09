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
    

    public RideSelectorUI rideSelectorUI; // ���� UI�� ����
    public RideDisplayUI rideDisplayUI;

    public Image gacharesultImage;

    // ��ư ������ ȣ��Ǵ� �޼���
    public void RollGacha()
    {
        StartCoroutine(GachaRoutine());
        resultText.text = "�α��α��α��α��α�";
    }

    IEnumerator GachaRoutine()
    {
        gacharesultImage.sprite = null;
        // ����Ʈ ����
        if (summonEffect != null)
            summonEffect.Play();

        // ���� �ð�
        yield return new WaitForSeconds(2f);



        // Ȯ�� ��� ��÷
        string result = RollResult();

        
        // ��Ī�� ��������Ʈ ã��
        Sprite rideSprite = rideDisplayUI.GetSpriteForRide(result);
        gacharesultImage.sprite = rideSprite;


        // ��� ǥ��
        resultText.text = "ȹ��: " + result;
        
        // �ر� ó��
        rideSelectorUI.UnlockRide(result);

        // UI ������Ʈ �ݿ� (���������� �ٽ� Start() ������ϰų� ���� �޼��� ����)
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