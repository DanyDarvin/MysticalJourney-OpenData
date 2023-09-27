using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using Npc.Enemy.Components;
using StaticData.Models;
using UnityEngine;
using UnityEngine.AI;
using static StaticData.Constants;

namespace Npc.Enemy.Ai.States
{
    public class EnemyPatrolState : IState
    {
        private readonly EnemyAiBehaviour _enemyAiBehaviour;
        private readonly NavMeshAgent _agent;
        private readonly IEnemy _entity;

        private int destPoint;

        public EnemyPatrolState(EnemyAiBehaviour enemyAiBehaviour)
        {
            _enemyAiBehaviour = enemyAiBehaviour;
            _entity = _enemyAiBehaviour.Entity();
            _agent = _entity.Agent();
        }

        public void OnEnter()
        {
            _entity.Agent().speed = _entity.PatrolMoveSpeed;
            _entity.AggroTriggerObserver().TriggerEnter += OnAggroTriggerEnter;
        }

        public void OnUpdate()
        {
            SetDestinationForAgent();
        }

        public void OnExit()
        {
            _entity.AggroTriggerObserver().TriggerEnter -= OnAggroTriggerEnter;
        }

        private void OnAggroTriggerEnter(Collider obj)
        {
            if (obj.CompareTag(PlayerTag) is false) return;

            SwitchChaseState();
        }

        private void SwitchChaseState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Chase);

        private void SetDestinationForAgent()
        {
            if (_enemyAiBehaviour.Entity().Health().IsNotEmpty &&
                _agent.isOnNavMesh &&
                _agent.pathPending is false &&
                _agent.remainingDistance < _entity.PatrolRemainingDistance)
            {
                GotoNextPoint();
            }
        }

        private void GotoNextPoint()
        {
            _agent.destination = _enemyAiBehaviour.Waypoints()[destPoint].Transform().position;
            destPoint = Random.Range(0, _enemyAiBehaviour.Waypoints().Length);
        }

        public void Dispose()
        {
            //
        }
    }
}