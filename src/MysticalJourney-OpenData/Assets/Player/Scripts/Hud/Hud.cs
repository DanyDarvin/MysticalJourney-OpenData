using Common.Player.Abstract.Components;
using Common.Player.Abstract.Hud;
using Common.Stats.Abstract;
using UniRx;
using UnityEngine;

namespace Player.Hud
{
    public class Hud : MonoBehaviour, IHud
    {
        [SerializeField] private ProgressBar _healthProgressBar;
        [SerializeField] private ProgressBar _armorProgressBar;
        [SerializeField] private ProgressBar _attackProgressBar;

        public Transform Transform() => transform;

        public void Initialize(IHealth health, IArmor armor, IPlayerAttack playerAttack)
        {
            health.Current.Subscribe(OnHealthChanged).AddTo(this);
            armor.Current.Subscribe(OnArmorChanged).AddTo(this);
            playerAttack.CooldownAmount.Subscribe(OnAttackCooldownChanged).AddTo(this);

            _healthProgressBar.Initialize(health.Current.Value, health.Max);
            _armorProgressBar.Initialize(armor.Current.Value, armor.Max);
            _attackProgressBar.Initialize(playerAttack.CooldownAmount.Value, playerAttack.CooldownAmount.Value);
        }

        private void OnHealthChanged(float current)
        {
            _healthProgressBar.SetValue(current);
        }

        private void OnArmorChanged(float current)
        {
            _armorProgressBar.SetValue(current);
        }

        private void OnAttackCooldownChanged(float current)
        {
            _attackProgressBar.SetValue(current);
        }
    }
}