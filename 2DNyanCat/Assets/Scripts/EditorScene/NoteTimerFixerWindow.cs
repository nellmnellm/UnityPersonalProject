using UnityEngine;
using UnityEditor;
using System.IO;

public class NoteTimerFixerWindow : EditorWindow
{
    private DefaultAsset jsonFile;
    private float shiftAmountInSeconds = 0.1f; // 전체 노트를 앞으로 얼마나 당길지 (초 단위)

    [MenuItem("Tools/Note Time Shifter")]
    static void OpenWindow()
    {
        GetWindow<NoteTimerFixerWindow>("Note Time Shifter");
    }

    void OnGUI()
    {
        jsonFile = (DefaultAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(DefaultAsset), false);
        shiftAmountInSeconds = EditorGUILayout.FloatField("Shift Time (- to delay, + to advance)", shiftAmountInSeconds);

        if (jsonFile != null && GUILayout.Button("Shift All Note Timings"))
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

            for (int i = 0; i < data.notes.Count; i++)
            {
                NoteData note = data.notes[i];
                note.time -= shiftAmountInSeconds;
                if (note.time < 0f) note.time = 0f; //음수 시간 방지
                data.notes[i] = note; // 리스트에 수정된 struct 다시 넣기
            }

            File.WriteAllText(fullPath, JsonUtility.ToJson(data, true));
            Debug.Log($"노트 시간 {shiftAmountInSeconds:F3}초 만큼 이동 완료!");
        }
    }
}