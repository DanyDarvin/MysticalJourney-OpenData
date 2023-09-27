using UniRx;
using UnityEngine;

namespace Npc.Enemy.Components
{
    public class EnemyEffector : MonoBehaviour
    {
        [SerializeField] private EnemyParent _enemyParent;
        [SerializeField] private ParticleSystem _hitVfx;
        [SerializeField] private GameObject _deathVfxPrefab;

        private void OnEnable()
        {
            _enemyParent.Health().Current.Subscribe(_ => { OnHealthChanged(); }).AddTo(this);
            _enemyParent.Death().OnRelease.Subscribe(OnReleased).AddTo(this);
        }

        private void OnHealthChanged()
        {
            _hitVfx.Play(true);
        }

        private void OnReleased(Unit unit)
        {
            //todo get from pool
            Instantiate(_deathVfxPrefab, transform.position, Quaternion.identity);
        }
    }
}