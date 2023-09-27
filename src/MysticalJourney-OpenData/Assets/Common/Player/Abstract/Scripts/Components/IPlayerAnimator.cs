using Common.Input.Abstract;

namespace Common.Player.Abstract.Components
{
    public interface IPlayerAnimator
    {
        void Construct(IInput inputService, IPlayer player);
        void Attacking();
    }
}