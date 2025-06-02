using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "Settings/Player Settings")]
public class PlayerSettingsData : ScriptableObject
{
    [Range(1f, 8f)]
    public float noteSpeed = 4.0f;         //��Ʈ ���ǵ� (0.5����?) �� ǥ���Ϸ��ߴµ� whole number ������ �� ǥ��� %2 �ؼ� 
    public bool showMV = true;             //����ϳ� ����ϳ�
    public float MVBright = 0.3f;             //�º� ���İ� ���� 
    public NoteSkinData selectedNoteSkin;  //��Ʈ ��� (��ǥ���� ����̸��)

    public float ScoreMultiplier => 0.8f + noteSpeed * 0.05f; //���� ���� ǥ�� (����� ������ ������� ����ġ)
    
}