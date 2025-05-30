using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Song Data")]
public class SongData : ScriptableObject
{
    //private int songIdx;         //�뷡 ������ȣ - �ϴ� �ʿ����
    public string songName;       //�뷡��
    public Sprite titleImage;     //Ÿ��Ʋ �̹���
    public string videoFileName;  //���� mp4 ���� �̸� (.mp4���� ǥ��)
    public float duration;        //�뷡 �ð�
    public string composer;       //�۰��
    public string difficulty;     //���̵��� ǥ�� (�������̾ ���̵� �ٸ��� ����)
}