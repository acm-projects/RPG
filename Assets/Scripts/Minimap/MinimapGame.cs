using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minimap;


public class MinimapGame : MonoBehaviour
{
    [SerializeField] private MinimapIcon playerMinimapIcon;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            MinimapWindow.Show();
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            MinimapWindow.Hide();
        }
    }
}
