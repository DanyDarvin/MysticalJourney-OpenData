using StaticData.Models;

namespace StaticData
{
    public interface IStaticData
    {
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
        PlayerStaticData ForPlayer();
        LevelStaticData ForLevel(string sceneKey);
        ScreenStaticData ForScreen(ScreenType screenType);
    }
}