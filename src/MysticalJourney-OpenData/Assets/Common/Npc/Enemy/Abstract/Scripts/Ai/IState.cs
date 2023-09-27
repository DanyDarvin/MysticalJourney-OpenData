using System;

namespace Common.Npc.Enemy.Abstract.Ai
{
    public interface IState : IDisposable
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }
}