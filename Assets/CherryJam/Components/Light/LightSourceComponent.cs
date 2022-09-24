using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CherryJam.Components.Light
{
    public class LightSourceComponent : MonoBehaviour
    {
        [SerializeField] private float _maxFuel = 30f;
        [SerializeField] private float _startGoOutValue = 6f;
        [SerializeField] private float _goOutSpeed = 0.001f;
        [SerializeField] private float _currentFuel;
        
        private Light2D _light;
        private bool _goesOut;
        private float _originalIntensity;
        
        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _currentFuel = _maxFuel;
            _originalIntensity = _light.intensity;
        }
        
        private void Update()
        {
            if (_goesOut && _light.intensity > 0)
                _light.intensity -= _goOutSpeed;

            if (_currentFuel <= 0) return;

            _currentFuel -= Time.deltaTime;
            _goesOut = _currentFuel < _startGoOutValue;
        }

        public void Refill()
        {
            _goesOut = false;
            _currentFuel = _maxFuel;
            _light.intensity = _originalIntensity;
        }
    }
}