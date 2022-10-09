using CherryJam.Creatures.Hero;
using UnityEngine;
using Cinemachine;

namespace CherryJam.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FollowCameraController : MonoBehaviour
    {
        private Hero _hero;
        private CinemachineVirtualCamera _vCamera;
        
        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
            
            _vCamera = GetComponent<CinemachineVirtualCamera>();
            _vCamera.Follow =_hero.transform;
        }
    }
}