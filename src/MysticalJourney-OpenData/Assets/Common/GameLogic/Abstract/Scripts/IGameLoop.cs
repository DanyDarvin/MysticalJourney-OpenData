using System;
using Cysharp.Threading.Tasks;

namespace Common.GameLogic.Abstract
{
    public interface IGameLoop : IDisposable
    {
        UniTask StartGameLevel(string sceneName);
        UniTask RetryCurrentLevel();
    }
}