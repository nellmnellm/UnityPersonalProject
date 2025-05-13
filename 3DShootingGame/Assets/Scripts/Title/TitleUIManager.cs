using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    public static TitleUIManager instance;

    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text speedText;
    public TMP_Text skillText;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayCharacterInfo(CharacterInfo info)
    {
        nameText.text = $"{info.characterName}";
        hpText.text = $"HP\n{info.maxHP}";
        speedText.text = $"Speed\n{info.Speed}";
        skillText.text = $"Skill\n{info.skillName}";
        
    }
}