using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyBindingUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text actionNameText;       // ���� ��
    public Button rebindButton;           // �����ε� ��ư
    public TMP_Text rebindButtonText;     // ��ư �� �ؽ�Ʈ

    [Header("Binding Info")]
    public string label; // �ν����Ϳ��� ������ ��
    public InputBindingsData bindingsData;
    public string actionFieldName; // ��: "attack1"
    public int bindingIndex; // �� ��° ���ε����� (��: 0 = S, 1 = D)

    private InputAction action;
    private void Start()
    {
        actionNameText.text = label;
        var field = bindingsData.GetType().GetField(actionFieldName, 
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var actionRef = field?.GetValue(bindingsData) as InputActionReference;
        if (actionRef != null)
        {
            action = actionRef.action;
            UpdateUI();
            rebindButton.onClick.AddListener(() => StartRebind());
        }
        else
        {
            Debug.LogError($"Action {actionFieldName} not found in InputBindingsData.");
        }
    }

    private void UpdateUI()
    {
        if (action != null)
            rebindButtonText.text = action.GetBindingDisplayString(bindingIndex);
    }

    private void StartRebind()
    {
        rebindButtonText.text = "...";

        action.Disable();

        action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnComplete(operation =>
            {
                operation.Dispose();
                action.Enable();
                UpdateUI();
            })
            .Start();
    }
}