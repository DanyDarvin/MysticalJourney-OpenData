using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StaticData.Models
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "StaticData/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        [Range(1f, 50f)] public float HP = 10f;
        [Range(1f, 50f)] public float Armor = 10f;

        [Range(0f, 100f)] public float AttackRange = 20f;
        [Range(0f, 100f)] public float VerticalRange = 20f;
        [Range(0f, 100f)] public float HorizontalRange = 0.4f;
        [Range(0f, 100f)] public float AttackRate = 2.7f;
        [Range(0f, 100f)] public float AttackDelay = 1.3f;
        [Range(0f, 100f)] public float AttackDamage = 1f;

        public AssetReferenceGameObject PlayerPrefabReference;
        public AssetReferenceGameObject HudPrefabReference;
    }
}