using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct NoteData
{
    public float time;
    [Range(-4.2f, 4.2f)]
    public float height; //yÃà À§Ä¡
}

public class ObjectSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public List<NoteData> notes = new List<NoteData>();
    public Transform noteParent;

    private const float noteSpeed = 5f;
    private const float judgeX = -2f;

    void Start()
    {
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
}