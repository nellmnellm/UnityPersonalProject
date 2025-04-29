using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEnhancer : MonoBehaviour
{
    public GameObject weaponObject; // OHS06 넣을 곳
    private Light weaponLight;       // 무기에 붙일 라이트
    private Coroutine enhanceRoutine;

    [Header("LogUI")]
    public Text logText;
    private Coroutine currentCoroutine;

    public Button enhanceButton;
    private GameObject failEffectPrefab;

    [Header("확률 관련")]
    public float baseSuccessRate = 0.8f; // 0레벨 기본 70%
    public float successDecreasePerLevel = 0.05f; // 레벨마다 5% 감소

    [Header("강화 정보")]
    public int weaponLevel = 0; // 현재 무기 레벨

    [Header("UI")]
    public TextMeshProUGUI successText;      // 성공 확률 표시용
    public TextMeshProUGUI failText;         // 실패 확률 표시용
    public TextMeshProUGUI levelText;
    void Start()
    {
        if (weaponObject == null)
        {
            Debug.LogError("WeaponEnhancer: weaponObject가 할당되지 않았습니다.");
        }
        failEffectPrefab = Resources.Load<GameObject>("Effect02");

        if (failEffectPrefab == null)
            Debug.LogError("WeaponEnhancer: Effect02 프리팹을 Resources 폴더에서 찾을 수 없습니다!");

        UpdateEnhanceUI();
    }

    public void ShowMessage(string message, float duration = 5f)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ShowMessageCoroutine(message, duration));
    }

    private IEnumerator ShowMessageCoroutine(string message, float duration)
    {
        logText.text = message;
        logText.enabled = true;

        yield return new WaitForSeconds(duration);

        logText.text = "";
        logText.enabled = false;
    }



    private void UpdateEnhanceUI()
    {
        float currentSuccessRate = GetCurrentSuccessRate();

        if (successText != null)
            successText.text = $"Success\n{currentSuccessRate * 100f}%";

        if (failText != null)
            failText.text = $"Failed\n{(1f - currentSuccessRate) * 100f}%";

        if (levelText != null)
            levelText.text = $"Weapon Level : {weaponLevel}";
    }


    private float GetCurrentSuccessRate()
    {
        float rate = baseSuccessRate - (weaponLevel * successDecreasePerLevel);
        return Mathf.Clamp(rate, 0f, 1f); // 최소 0%, 최대 100%로 제한
    }


    public void EnhanceWeapon()
    {
        if (weaponObject == null)
            return;

        // 강화 성공 여부 결정
        bool isSuccess = Random.value <= GetCurrentSuccessRate(); // 0~1 랜덤값과 비교

        if (enhanceRoutine != null)
        {
            StopCoroutine(enhanceRoutine);
        }
        enhanceRoutine = StartCoroutine(BlinkLight(isSuccess));
    }

    private IEnumerator BlinkLight(bool isSuccess)
    {
        // 라이트 초기화
        weaponLight = weaponObject.GetComponent<Light>();
        if (weaponLight == null)
        {
            weaponLight = weaponObject.AddComponent<Light>();
            weaponLight.type = LightType.Point;
            weaponLight.intensity = 5f;
            weaponLight.range = 2f;
            weaponLight.enabled = false;
        }

        float duration = 1f;
        float blinkInterval = 0.1f;
        float timer = 0f;

        while (timer < duration)
        {
            weaponLight.enabled = !weaponLight.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // 강화 결과 처리
        if (isSuccess)
        {
            weaponLight.enabled = true; // 성공: 빛 유지
            weaponLevel++;
            ChangeWeaponLightColor();
            UpdateEnhanceUI();
            Debug.Log($"강화 성공! {weaponLevel}");
            ShowMessage($"강화 성공! {weaponLevel}레벨 무기가 되었습니다.", 5f);
        }
        else
        {
            weaponLevel = 0;
            weaponLight.enabled = false;
            weaponObject.SetActive(false);
            UpdateEnhanceUI();
            Debug.Log("강화 실패... 무기가 사라졌습니다.");
            ShowMessage("강화 실패...! 무기가 사라졌습니다.", 5f);
            if (enhanceButton != null)
            {
                enhanceButton.interactable = false; // 버튼 비활성화
            }
            if (failEffectPrefab != null)
            {
                Instantiate(failEffectPrefab, weaponObject.transform.position, Quaternion.identity);
            }
        }
    }

    private void ChangeWeaponLightColor()
    {
        if (weaponLight == null)
            return;

        switch (weaponLevel)
        {
            case 0:
                weaponLight.color = Color.white;
                break;
            case 1:
                weaponLight.color = Color.yellow;
                break;
            case 2:
                weaponLight.color = Color.green;
                break;
            case 3:
                weaponLight.color = Color.blue;
                break;
            case 4:
                weaponLight.color = new Color(0.5f, 0f, 1f); // 보라색
                break;
            default:
                weaponLight.color = Color.red; // 5레벨 이상
                break;
        }
    }

    public void Level0Weapon()
    {
        if (!weaponObject.activeSelf)
        {
            weaponObject.SetActive(true);
            weaponLevel = 0;
            if (weaponLight != null)
            {
                weaponLight.enabled = false;
                weaponLight.color = Color.white;
            }

            if (enhanceButton != null)
            {
                enhanceButton.interactable = true; // 버튼 활성화
            }

            UpdateEnhanceUI();
            Debug.Log("0레벨 무기로 복구되었습니다!");
            ShowMessage("0레벨 무기로 복구되었습니다!", 5f);
        }
    }
}