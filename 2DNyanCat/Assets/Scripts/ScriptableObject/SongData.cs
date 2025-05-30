using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Song Data")]
public class SongData : ScriptableObject
{
    //private int songIdx;         //노래 고유번호 - 일단 필요없음
    public string songName;       //노래명
    public Sprite titleImage;     //타이틀 이미지
    public string videoFileName;  //넣은 mp4 파일 이름 (.mp4까지 표기)
    public float duration;        //노래 시간
    public string composer;       //작곡가명
    public string difficulty;     //난이도명 표시 (같은곡이어도 난이도 다르게 ㄱㄱ)
}