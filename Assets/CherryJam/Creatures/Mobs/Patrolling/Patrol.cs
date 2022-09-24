using System.Collections;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
