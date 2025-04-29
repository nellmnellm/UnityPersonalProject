using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEnhancer : MonoBehaviour
{
    public GameObject weaponObject; // OHS06 ���� ��
    private Light weaponLight;       // ���⿡ ���� ����Ʈ
    private Coroutine enhanceRoutine;

    [Header("LogUI")]
    public Text logText;
    private Coroutine currentCoroutine;

    public Button enhanceButton;
    private GameObject failEffectPrefab;

    [Header("Ȯ�� ����")]
    public float baseSuccessRate = 0.8f; // 0���� �⺻ 70%
    public float successDecreasePerLevel = 0.05f; // �������� 5% ����

    [Header("��ȭ ����")]
    public int weaponLevel = 0; // ���� ���� ����

    [Header("UI")]
    public TextMeshProUGUI successText;      // ���� Ȯ�� ǥ�ÿ�
    public TextMeshProUGUI failText;         // ���� Ȯ�� ǥ�ÿ�
    public TextMeshProUGUI levelText;
    void Start()
    {
        if (weaponObject == null)
        {
            Debug.LogError("WeaponEnhancer: weaponObject�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        failEffectPrefab = Resources.Load<GameObject>("Effect02");

        if (failEffectPrefab == null)
            Debug.LogError("WeaponEnhancer: Effect02 �������� Resources �������� ã�� �� �����ϴ�!");

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
        return Mathf.Clamp(rate, 0f, 1f); // �ּ� 0%, �ִ� 100%�� ����
    }


    public void EnhanceWeapon()
    {
        if (weaponObject == null)
            return;

        // ��ȭ ���� ���� ����
        bool isSuccess = Random.value <= GetCurrentSuccessRate(); // 0~1 �������� ��

        if (enhanceRoutine != null)
        {
            StopCoroutine(enhanceRoutine);
        }
        enhanceRoutine = StartCoroutine(BlinkLight(isSuccess));
    }

    private IEnumerator BlinkLight(bool isSuccess)
    {
        // ����Ʈ �ʱ�ȭ
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

        // ��ȭ ��� ó��
        if (isSuccess)
        {
            weaponLight.enabled = true; // ����: �� ����
            weaponLevel++;
            ChangeWeaponLightColor();
            UpdateEnhanceUI();
            Debug.Log($"��ȭ ����! {weaponLevel}");
            ShowMessage($"��ȭ ����! {weaponLevel}���� ���Ⱑ �Ǿ����ϴ�.", 5f);
        }
        else
        {
            weaponLevel = 0;
            weaponLight.enabled = false;
            weaponObject.SetActive(false);
            UpdateEnhanceUI();
            Debug.Log("��ȭ ����... ���Ⱑ ��������ϴ�.");
            ShowMessage("��ȭ ����...! ���Ⱑ ��������ϴ�.", 5f);
            if (enhanceButton != null)
            {
                enhanceButton.interactable = false; // ��ư ��Ȱ��ȭ
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
                weaponLight.color = new Color(0.5f, 0f, 1f); // �����
                break;
            default:
                weaponLight.color = Color.red; // 5���� �̻�
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
                enhanceButton.interactable = true; // ��ư Ȱ��ȭ
            }

            UpdateEnhanceUI();
            Debug.Log("0���� ����� �����Ǿ����ϴ�!");
            ShowMessage("0���� ����� �����Ǿ����ϴ�!", 5f);
        }
    }
}