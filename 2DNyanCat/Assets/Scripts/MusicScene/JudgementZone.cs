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
        Note earliest = null;
        float minTime = float.MaxValue;

        foreach (var note in overlappingNotes)
        {
            if (note.HitTime < minTime)
            {
                minTime = note.HitTime;
                earliest = note;
            }
        }

        return earliest;
    }

}