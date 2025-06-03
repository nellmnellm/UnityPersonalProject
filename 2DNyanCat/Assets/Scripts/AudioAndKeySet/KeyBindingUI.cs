using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyBindingUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text actionNameText;       // 좌측 라벨
    public Button rebindButton;           // 리바인드 버튼
    public TMP_Text rebindButtonText;     // 버튼 내 텍스트

    [Header("Binding Info")]
    public string label; // 인스펙터에서 설정할 라벨
    public InputBindingsData bindingsData;
    public string actionFieldName; // 예: "attack1"
    public int bindingIndex; // 몇 번째 바인딩인지 (예: 0 = S, 1 = D)

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