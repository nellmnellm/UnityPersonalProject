using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "Settings/Player Settings")]
public class PlayerSettingsData : ScriptableObject
{
    [Range(1f, 8f)]
    public float noteSpeed = 4.0f;         //노트 스피드 (0.5단위?) 로 표기하려했는데 whole number 쓰려고 함 표기는 %2 해서 
    public bool showMV = true;             //토글하나 밝기하나
    public float MVBright = 0.3f;             //뮤비 알파값 조절 
    public NoteSkinData selectedNoteSkin;  //노트 모양 (음표모양과 고양이모양)

    public float ScoreMultiplier => 0.8f + noteSpeed * 0.05f; //점수 배율 표시 (배속이 높으면 어려워짐 보정치)
    
}