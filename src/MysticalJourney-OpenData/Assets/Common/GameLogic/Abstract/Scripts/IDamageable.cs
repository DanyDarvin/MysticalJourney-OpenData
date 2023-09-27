using UnityEngine;

namespace Common.GameLogic.Abstract
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector3 point);
    }
}