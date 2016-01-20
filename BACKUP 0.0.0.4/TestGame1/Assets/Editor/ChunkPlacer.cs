using UnityEngine;
using UnityEditor;
using System.Collections;
using Assets.Scripts;

namespace Assets.Editor
{
    [CustomEditor(typeof(Chunk))]
    public class ObjectBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Chunk myScript = (Chunk)target;
            if (GUILayout.Button("Build Chunk"))
            {
                myScript.GetComponent<MeshFilter>().mesh = null;
                myScript.GetComponent<MeshCollider>().sharedMesh = null;
                myScript.Create();
            }
            if(GUILayout.Button("Test Chunk"))
            {
                myScript.Start();
            }
        }
    }
}
