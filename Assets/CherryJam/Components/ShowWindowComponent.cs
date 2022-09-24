using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Components
{
    public class ShowWindowComponent : MonoBehaviour
    {
        [SerializeField] private string _path;
        
        public void Show()
        {
            WindowUtils.CreateWindow(_path);
        }
    }
}