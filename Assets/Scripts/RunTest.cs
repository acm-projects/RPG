using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTest : MonoBehaviour
{

    public RoomFirstDungeonGen roomFirstDungeonGen;

    // Start is called before the first frame update
    void Start()
    {
        roomFirstDungeonGen.RunGeneration();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

