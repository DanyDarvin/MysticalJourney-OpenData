using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StaticData.Models
{
    [CreateAssetMenu(fileName = "ScreenConfig", menuName = "StaticData/Screen")]
    public class ScreenStaticData : ScriptableObject
    {
        public ScreenType ScreenType;
        public AssetReferenceGameObject PrefabReference;
    }
}