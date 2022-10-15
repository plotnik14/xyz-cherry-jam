using UnityEngine;

namespace CherryJam.Utils
{
    public static class WindowUtils
    {
        public static GameObject CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = GameObject.FindWithTag("MainUICanvas").GetComponent<Canvas>();
            return Object.Instantiate(window, canvas.transform);
        }
    }
}