using System.Collections.Generic;
using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using Npc.Enemy.Ai.States;
using Npc.Enemy.Components;
using StaticData.Models;

namespace Npc.Enemy.Ai
{
    public class EnemyAiBehaviour : IAiBehaviour<IEnemy>
    {
        public IEnemy Entity() => _enemy;
        public IState CurrentState() => _currentState;
        public IWaypoint[] Waypoints() => _waypoints;

        private Dictionary<EnemyStateType, IState> _enemyStates;
        private IState _currentState;
        private IEnemy _enemy;
        private IWaypoint[] _waypoints;

        public void Initialize(IEnemy entity, IWaypoint[] waypoints)
        {
            _waypoints = waypoints;
            _enemy = entity;

            _enemyStates = new Dictionary<EnemyStateType, IState>
            {
                { EnemyStateType.Chase, new EnemyChaseState(this) },
                { EnemyStateType.Idle, new EnemyIdleState(this) },
                { EnemyStateType.Hurt, new EnemyHurtState(this) },
                { EnemyStateType.Patrol, new EnemyPatrolState(this) },
            };

            ChangeState(EnemyStateType.Idle);
        }

        public void ChangeState(EnemyStateType enemyStateType)
        {
            _currentState?.OnExit();
            _currentState = _enemyStates[enemyStateType];
            _currentState.OnEnter();
        }

        public void Tick()
        {
            _currentState?.OnUpdate();
        }

        public void Dispose()
        {
            if (_enemyStates is null) return;
            foreach (var enemyState in _enemyStates.Values)
            {
                enemyState.Dispose();
            }

            _enemyStates.Clear();
        }
    }
}