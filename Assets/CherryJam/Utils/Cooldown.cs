using System;
using UnityEngine;

namespace CherryJam.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;

        private float _timesUp;

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public void Reset()
        {
            _timesUp = Time.time + _value;
        }

        public bool IsReady => _timesUp <= Time.time;

        public float RemainingTime => Mathf.Max(_timesUp - Time.time, 0);
    }
}
