using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    public PlayerSettingsData playerSettings; // ScriptableObject ����

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ���� ��Ƴ���
    }
}