using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGen),true)]
public class RandomDungeonGenEditor : Editor
{
    AbstractDungeonGen gen;
    
    private void Awake()
    {
        gen = (AbstractDungeonGen)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            gen.GenerateDungeon();
        }
    }
}
