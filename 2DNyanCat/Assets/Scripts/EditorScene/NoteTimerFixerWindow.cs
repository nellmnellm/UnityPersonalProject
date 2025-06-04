using UnityEngine;
using UnityEditor;
using System.IO;

public class NoteTimerFixerWindow : EditorWindow
{
    private DefaultAsset jsonFile;
    private float shiftAmountInSeconds = 0.1f; // ��ü ��Ʈ�� ������ �󸶳� ����� (�� ����)

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
                Debug.LogError("������ ������ .json ������ �ƴմϴ�.");
                return;
            }

            string fullPath = Path.Combine(
                Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                assetPath);
            Debug.Log("���� ���: " + fullPath);

            string jsonText = File.ReadAllText(fullPath);
            NoteDataList data = JsonUtility.FromJson<NoteDataList>(jsonText);

            for (int i = 0; i < data.notes.Count; i++)
            {
                NoteData note = data.notes[i];
                note.time -= shiftAmountInSeconds;
                if (note.time < 0f) note.time = 0f; //���� �ð� ����
                data.notes[i] = note; // ����Ʈ�� ������ struct �ٽ� �ֱ�
            }

            File.WriteAllText(fullPath, JsonUtility.ToJson(data, true));
            Debug.Log($"��Ʈ �ð� {shiftAmountInSeconds:F3}�� ��ŭ �̵� �Ϸ�!");
        }
    }
}