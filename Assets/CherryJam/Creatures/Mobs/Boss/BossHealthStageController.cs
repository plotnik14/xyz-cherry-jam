using System;
using System.Collections.Generic;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss
{
    public class BossHealthStageController : MonoBehaviour
    {
        [SerializeField] private List<BossHealthStage> _healthValueList;

        private int _stageIndex = 0;
        
        public bool HasReachedNextStage(int currentHealth, int maxHealth)
        {
            if (_stageIndex >= _healthValueList.Count) return false;

            var healthLeftInPercent = (float) currentHealth / maxHealth * 100;
            var stageThreshold = _healthValueList[_stageIndex].HealthPercent;

            if (healthLeftInPercent <= stageThreshold)
            {
                _stageIndex++;
                return true;
            }
            
            return false;
        }
    }

    [Serializable]
    public struct BossHealthStage
    {
        [SerializeField] [Range(0, 100)] private int _healthPercent;

        public int HealthPercent => _healthPercent;
    }
}