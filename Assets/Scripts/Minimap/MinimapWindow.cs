using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimap
{
    public class MinimapWindow : MonoBehaviour
    {
        private static MinimapWindow instance;

        public void Awake()
        {
            instance = this;
        }

        public static void Show()
        {
            instance.gameObject.SetActive(true);
        }
        public static void Hide()
        {
            instance.gameObject.SetActive(false);
        }
    }
}
