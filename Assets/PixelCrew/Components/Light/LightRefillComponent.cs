using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.Light
{
    public class LightRefillComponent : MonoBehaviour
    {
        // [SerializeField] private float _value; // Refill partially depending on the fuel source?

        private Hero _hero;
        
        public void Refill(GameObject target)
        {
            if (_hero == null)
                _hero = target.GetComponent<Hero>();

            _hero.RefillLightFuel();
        }
    }
}