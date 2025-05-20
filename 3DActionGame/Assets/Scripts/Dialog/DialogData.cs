using UnityEditor;
using UnityEngine;


public class DialogData
{
    public DialogType Type { get; set; }

    public DialogData(DialogType type)
    {
        Type = type;
    }
}
