using Common.Npc.Enemy.Abstract.Ai;
using UnityEngine;

namespace Npc.Enemy.Ai
{
    public class Waypoint : MonoBehaviour, IWaypoint
    {
        public Transform Transform() => transform;
    }
}