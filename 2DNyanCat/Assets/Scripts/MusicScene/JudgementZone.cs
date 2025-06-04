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
            return note; // ���� ���� ���� ��Ʈ 1���� ��ȯ
        }
        return null;
    }

}