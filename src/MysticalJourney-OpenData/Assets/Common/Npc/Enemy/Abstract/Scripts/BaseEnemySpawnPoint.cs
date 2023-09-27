using Common.Npc.Enemy.Abstract.Ai;
using StaticData.Models;
using UnityEngine;

namespace Common.Npc.Enemy.Abstract
{
    public abstract class BaseEnemySpawnPoint : MonoBehaviour
    {
        public abstract EnemyTypeId EnemyTypeId();
        public abstract IWaypoint[] Waypoints();
    }
}