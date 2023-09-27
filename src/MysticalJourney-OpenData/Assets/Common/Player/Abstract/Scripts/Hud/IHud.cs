using Common.Components.Abstract;
using Common.Player.Abstract.Components;
using Common.Stats.Abstract;

namespace Common.Player.Abstract.Hud
{
    public interface IHud : IParent
    {
        void Initialize(IHealth health, IArmor armor, IPlayerAttack playerAttack);
    }
}