using System;
using Common.Npc.Enemy.Abstract;
using Common.Npc.Enemy.Abstract.Ai;
using Npc.Enemy.Ai;
using StaticData.Models;
using UnityEngine;

namespace Npc.Enemy.Spawners
{
    [Serializable]
    public class EnemySpawnPoint : BaseEnemySpawnPoint
    {
        [SerializeField] private EnemyTypeId _enemyTypeId;
        [SerializeField] private Waypoint[] _waypoints;

        public override EnemyTypeId EnemyTypeId() => _enemyTypeId;
        public override IWaypoint[] Waypoints() => _waypoints;
    }
}