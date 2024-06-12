using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Minimap
{
    public class MinimapIcon : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
