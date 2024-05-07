using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LvLGenerationController))]
public class LevelGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LvLGenerationController generator = (LvLGenerationController)target;

        if (GUILayout.Button("Generate New Matrix"))
        {
            generator.GenerateNewMatrix();
        }
    }
}

