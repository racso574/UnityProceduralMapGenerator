using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LvlGenerationController))]
public class LevelGenerationButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LvlGenerationController generator = (LvlGenerationController)target;

        if (GUILayout.Button("Generate New Matrix"))
        {
            generator.GenerateNewMatrix();
        }

        if (GUILayout.Button("Clear Matrix"))
        {
            LvlSchemaMapGeneration mapGenerator = generator.GetComponent<LvlSchemaMapGeneration>();
            if (mapGenerator != null)
            {
                mapGenerator.ClearMap();
            }
            else
            {
                Debug.LogWarning("LvlSchemaMapGeneration component not found on the same GameObject.");
            }
        }
    }
}




