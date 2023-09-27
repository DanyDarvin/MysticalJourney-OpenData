using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StaticData.Models
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;

        [Range(1f, 100f)] public int Hp = 10;
        [Range(1f, 30f)] public float Damage = 3f;

        [Range(0f, 10f)] public float PatrolMoveSpeed = 3f;
        [Range(0f, 10f)] public float ChaseMoveSpeed = 3.3f;
        [Range(0f, 10f)] public float MinimalChaseDistance = 1f;

        [Range(0.5f, 10f)] public float EffectiveDistance = 0.5f;
        [Range(0.5f, 10f)] public float Cleavage = 0.5f;
        [Range(0.5f, 10f)] public float PatrolRemainingDistance = 0.5f;
        [Range(0.5f, 10f)] public float AttackCooldown = 2f;
        [Range(0.5f, 10f)] public float IdleCooldown = 2f;

        public AssetReferenceGameObject PrefabReference;
    }
}