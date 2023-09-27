using Common.Components.Abstract;
using Common.GameLogic.Abstract;
using Common.Npc.Enemy.Abstract.Ai;
using Common.Player.Abstract.Components;
using Common.Stats.Abstract;
using StaticData.Models;
using UnityEngine.AI;

namespace Common.Npc.Enemy.Abstract.Components
{
    public interface IEnemy : IParent, IDamageable
    {
        EnemyTypeId EnemyTypeId { get; }
        float MinimalChaseDistance { get; set; }
        float Damage { get; set; }
        float AttackCooldown { get; set; }
        float IdleCooldown { get; set; }
        double PatrolRemainingDistance { get; set; }
        float PatrolMoveSpeed { get; set; }
        float ChaseMoveSpeed { get; set; }
        void Initialize(EnemyTypeId enemyTypeId, IPlayer player, IWaypoint[] waypoints);
        ITriggerObserver AggroTriggerObserver();
        ITriggerObserver HurtTriggerObserver();
        IEnemyAnimator Animator();
        IEnemyAudioFx AudioFx();
        NavMeshAgent Agent();
        IAiBehaviour<IEnemy> AiBehaviour();
        IHealth Health();
        IEnemyDeath Death();
        IPlayer Player();
    }
}