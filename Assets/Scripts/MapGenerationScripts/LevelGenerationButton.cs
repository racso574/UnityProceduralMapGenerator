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
    }
}



