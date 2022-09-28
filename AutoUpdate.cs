using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(GenRoads))]
public class AutoUpdate : Editor
{
    public override void OnInspectorGUI()
    {
        GenRoads gen = (GenRoads)target;

        //DrawDefaultInspector();
        if (DrawDefaultInspector()) gen.Generate();

        if (GUILayout.Button("Generate")) gen.Generate();
    }
}