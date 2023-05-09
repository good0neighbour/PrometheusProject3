using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonCreator))]
public class JsonCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        JsonCreator lan = (JsonCreator)target;
        if (GUILayout.Button("Save Korean.Json, Translate.txt"))
        {
            lan.LanSave();
        }
    }
}