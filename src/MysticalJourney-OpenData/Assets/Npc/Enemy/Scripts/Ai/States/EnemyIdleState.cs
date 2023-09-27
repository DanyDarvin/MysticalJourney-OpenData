using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using DG.Tweening;
using Npc.Enemy.Components;
using StaticData.Models;
using UnityEngine;
using UnityEngine.AI;
using static StaticData.Constants;

namespace Npc.Enemy.Ai.States
{
    public class EnemyIdleState : IState
    {
        private readonly EnemyAiBehaviour _enemyAiBehaviour;
        private readonly NavMeshAgent _agent;
        private readonly IEnemy _entity;

        private Tweener _idleCooldownTweener;

        public EnemyIdleState(EnemyAiBehaviour enemyAiBehaviour)
        {
            _enemyAiBehaviour = enemyAiBehaviour;
            _entity = _enemyAiBehaviour.Entity();
            _agent = _enemyAiBehaviour.Entity().Agent();
        }

        public void OnEnter()
        {
            _entity.AggroTriggerObserver().TriggerEnter += OnAggroTriggerEnter;

            SetDestinationForAgent();

            IdleCooldown();
        }

        public void OnUpdate()
        {
            //
        }

        public void OnExit()
        {
            _entity.AggroTriggerObserver().TriggerEnter -= OnAggroTriggerEnter;
            _idleCooldownTweener.Kill();
        }

        private void IdleCooldown()
        {
            _idleCooldownTweener = DOVirtual
                .Float(0f, _entity.IdleCooldown, _entity.IdleCooldown, _ => { })
                .OnComplete(SwitchPatrolState)
                .SetAutoKill(true);
        }

        private void OnAggroTriggerEnter(Collider obj)
        {
            if (obj.CompareTag(PlayerTag) is false) return;

            SwitchChaseState();
        }

        private void SwitchChaseState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Chase);
        private void SwitchPatrolState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Patrol);

        private void SetDestinationForAgent()
        {
            if (_enemyAiBehaviour.Entity().Health().IsNotEmpty &&
                _agent.isOnNavMesh)
                _agent.destination = _enemyAiBehaviour.Entity().Transform().position;
        }

        public void Dispose()
        {
            _idleCooldownTweener?.Kill();
        }
    }
}