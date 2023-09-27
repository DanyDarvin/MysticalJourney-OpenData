using System;
using Common.Components.Abstract;
using StaticData.Models;
using Zenject;

namespace Common.Npc.Enemy.Abstract.Ai
{
    public interface IAiBehaviour<TEntity> : IDisposable, ITickable where TEntity : IParent
    {
        TEntity Entity();
        IState CurrentState();
        IWaypoint[] Waypoints();
        void Initialize(TEntity entity, IWaypoint[] waypoints);
        void ChangeState(EnemyStateType enemyStateType);
    }
}