using System.Collections.Generic;
using UnityEngine;

public class JudgeZone : MonoBehaviour
{
    private HashSet<Note> overlappingNotes = new HashSet<Note>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            var note = other.GetComponent<Note>();
            if (note != null)
                overlappingNotes.Add(note);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            var note = other.GetComponent<Note>();
            if (note != null)
                overlappingNotes.Remove(note);
        }
    }

    public Note GetFirstNote()
    {
        foreach (var note in overlappingNotes)
        {
            return note; // 가장 먼저 들어온 노트 1개만 반환
        }
        return null;
    }

}