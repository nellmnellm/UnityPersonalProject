using UnityEngine;
using UnityEditor;
using System.IO;

public class NoteFixerWindow : EditorWindow
{
    private DefaultAsset jsonFile;
    private float bpm = 120f;
    private int quantizeUnit = 4;

    [MenuItem("Tools/Note Fixer")]
    static void OpenWindow()
    {
        GetWindow<NoteFixerWindow>("Note Fixer");
    }

    void OnGUI()
    {
        jsonFile = (DefaultAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(DefaultAsset), false);
        bpm = EditorGUILayout.FloatField("BPM", bpm);
        quantizeUnit = EditorGUILayout.IntField("Quantize Unit", quantizeUnit);

        if (jsonFile != null && GUILayout.Button("Fix Note Timings"))
        {
            string assetPath = AssetDatabase.GetAssetPath(jsonFile);

            if (!assetPath.EndsWith(".json"))
            {
                Debug.LogError("선택한 파일이 .json 파일이 아닙니다.");
                return;
            }

            string fullPath = Path.Combine(
            Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
            assetPath);
            Debug.Log("실제 경로: " + fullPath);

            string jsonText = File.ReadAllText(fullPath);

            NoteDataList data = JsonUtility.FromJson<NoteDataList>(jsonText);

            float interval = 60f / bpm / quantizeUnit;
            float baseTime = data.notes[0].time;

            for (int i = 0; i < data.notes.Count; i++)
            {
                NoteData note = data.notes[i];
                float offset = note.time - baseTime;
                int step = Mathf.RoundToInt(offset / interval);
                note.time = baseTime + step * interval;
                data.notes[i] = note;
            }

            File.WriteAllText(fullPath, JsonUtility.ToJson(data, true));
            Debug.Log("노트 시간 양자화 완료!");
        }
    }
}