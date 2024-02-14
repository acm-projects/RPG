using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SimpleRandomWalkParameters_",menuName = "PCG/SimpleRAndomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    //defaulting to a pretty small dungeon
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
}