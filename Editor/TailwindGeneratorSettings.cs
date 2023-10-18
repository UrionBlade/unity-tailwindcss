using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "TailwindGeneratorSettings", menuName = "TailwindGeneratorSettings")]
public class TailwindGeneratorSettings : ScriptableObject
{
    public string jsonFilePath = "Packages/it.watermelon.tailwindcss/Editor/tailwind.json";
    public string ussFilePath = "Assets/Styles/tailwind.uss";
    public string jsonExtFilePath = "";
}

#if UNITY_EDITOR
[CustomEditor(typeof(TailwindGeneratorSettings))]
public class TailwindGeneratorSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var settings = (TailwindGeneratorSettings)target;

        if (!string.IsNullOrWhiteSpace(settings.ussFilePath))
        {
            var directoryName = Path.GetDirectoryName(settings.ussFilePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        if (GUILayout.Button("Generate"))
        {
            TailwindGenerator.Generate(
                settings.ussFilePath,
                settings.jsonFilePath,
                settings.jsonExtFilePath
            );
            AssetDatabase.Refresh();
        }
    }
}
#endif