using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



[System.Serializable]
public struct NoteData
{
    public float time;
    [Range(-4.2f, 4.2f)]
    public float height; //y축 위치
}

public class ObjectSpawner : MonoBehaviour
{
    private GameObject notePrefab;
    // public List<NoteData> notes = new List<NoteData>();   //tester용
    public Transform noteParent;

    private const float noteSpeed = 5f; // 나중에 송셀렉트에서 인자로 받을예정
    private const float judgeX = -2f;   // 캐릭터의 x 위치가 -2임

    void Start()
    {
        var skin = SettingManager.Instance.playerSettings.selectedNoteSkin;
        notePrefab = skin.notePrefab;

        List<NoteData> notes = LoadNotesFromJson();
        foreach (var note in notes)
        {
            float spawnX = note.time * noteSpeed + judgeX;
            Vector3 spawnPos = new Vector3(spawnX, note.height, 0f);
            GameObject noteObj = Instantiate(notePrefab, spawnPos, Quaternion.identity, noteParent);

            Note move = noteObj.GetComponent<Note>();
            if (move != null)
            {
                move.Init(note.time);
            }
        }
    }

    private List<NoteData> LoadNotesFromJson()
    {
        if (SongLoader.SelectedSong == null)
        {
            Debug.LogError("SongLoader.SelectedSong이 비어있습니다.");
            return new List<NoteData>();
        }

        string baseName = Path.GetFileNameWithoutExtension(SongLoader.SelectedSong.videoFileName);
        string path = Path.Combine(Application.streamingAssetsPath, "Charts", baseName + ".json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("해당 JSON 채보 파일이 존재하지 않습니다: " + path);
            return new List<NoteData>();
        }

        string json = File.ReadAllText(path);
        NoteDataList data = JsonUtility.FromJson<NoteDataList>(json);
        return data.notes;
    }

}