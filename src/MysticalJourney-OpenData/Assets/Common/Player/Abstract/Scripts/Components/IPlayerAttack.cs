using UniRx;

namespace Common.Player.Abstract.Components
{
    public interface IPlayerAttack
    {
        IReadOnlyReactiveProperty<float> CooldownAmount { get; }

        void Initialize(
            IPlayer player,
            float range,
            float verticalRange,
            float horizontalRange,
            float attackRate,
            float attackDelay,
            float attackDamage
        );

        void Enable();
        void Disable();
    }
}