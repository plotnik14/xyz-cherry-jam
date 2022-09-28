using System;
using UnityEngine;

namespace CherryJam.Components
{
    public class ActivateCreds : MonoBehaviour
    {
        private void Start()
        {
            var container = GameObject.FindObjectOfType<ActivateCredsContrl>();
            container.Activate();
        }
    }
}