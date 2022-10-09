using System;
using System.Collections.Generic;
using UnityEngine;

namespace CherryJam.Components.LevelManagement.SpawnPoints
{
    public class SpawnPointsConfigComponent : MonoBehaviour
    {
        [SerializeField] private List<SpawnPointComponent> _points;

        public SpawnPointComponent GetPointByType(SpawnPointType type)
        {
            foreach (var point in _points)
            {
                if (point.Type == type)
                    return point;
            }

            throw new IndexOutOfRangeException("Spawn configuration does not contain such type of point");
        }
    }
}