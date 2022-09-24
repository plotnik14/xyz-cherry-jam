using CherryJam.Creatures.Hero;
using UnityEngine;
using Cinemachine;

namespace CherryJam.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = FindObjectOfType<Hero>().transform;
        }
    }
}