﻿using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Components.ColliderBased
{
    public class ExitTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;
            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;
            
             _action?.Invoke(other.gameObject);       
        }
    }
}
