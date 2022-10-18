using UnityEngine;

namespace CherryJam.Utils
{
    public static class TimeUtils
    {
        private static float _timeScaleBeforeStop = -1;
        
        public static void StopTime()
        {
            if (_timeScaleBeforeStop >= 0) return;
            
            _timeScaleBeforeStop = Time.timeScale;
            Time.timeScale = 0;
        }

        public static void ResumeTime()
        {
            if (_timeScaleBeforeStop <= 0) return;

            Time.timeScale = _timeScaleBeforeStop;
            _timeScaleBeforeStop = -1;
        }
    }
}