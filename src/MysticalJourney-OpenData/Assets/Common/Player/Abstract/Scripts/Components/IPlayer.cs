using Common.Components.Abstract;
using Common.GameLogic.Abstract;
using Common.Stats.Abstract;
using UnityEngine;

namespace Common.Player.Abstract.Components
{
    public interface IPlayer : IParent, IDamageable
    {
        IPlayerAnimator Animator();
        IPlayerAudioFx AudioFx();
        Camera Camera();
        CharacterController CharacterController();
        IPlayerAttack PlayerAttack();
        IHealth Health();
        IArmor Armor();
    }
}