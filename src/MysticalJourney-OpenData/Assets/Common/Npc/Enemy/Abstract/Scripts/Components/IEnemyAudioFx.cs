namespace Common.Npc.Enemy.Abstract.Components
{
    public interface IEnemyAudioFx
    {
        void Hitting();
        void Attacking();
        void Idling();
    }
}