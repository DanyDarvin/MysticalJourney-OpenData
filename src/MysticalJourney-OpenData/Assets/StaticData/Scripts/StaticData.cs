using System.Collections.Generic;
using System.Linq;
using StaticData.Models;
using UnityEngine;
using Zenject;
using static StaticData.Constants;

namespace StaticData
{
    public class StaticData : IStaticData, IInitializable
    {
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<ScreenType, ScreenStaticData> _screens;

        private PlayerStaticData _player;

        public void Initialize()
        {
            Load();
        }

        public EnemyStaticData ForEnemy(EnemyTypeId typeId) =>
            _enemies.TryGetValue(typeId, out var staticData) ? staticData : null;

        public PlayerStaticData ForPlayer() => _player;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out var levelStaticData) ? levelStaticData : null;

        public ScreenStaticData ForScreen(ScreenType screenType) =>
            _screens.TryGetValue(screenType, out var screenStaticData) ? screenStaticData : null;

        private void Load()
        {
            _player = Resources.Load<PlayerStaticData>(PlayerDataPath);

            _enemies = Resources
                .LoadAll<EnemyStaticData>(EnemiesDataPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);

            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.LevelKey, x => x);

            _screens = Resources
                .LoadAll<ScreenStaticData>(ScreensDataPath)
                .ToDictionary(x => x.ScreenType, x => x);
        }
    }
}