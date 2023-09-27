using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Ui.Screens.Factories;
using Common.Ui.Screens.Mediators;
using Cysharp.Threading.Tasks;
using StaticData.Models;
using UnityEngine;
using Zenject;

namespace Common.Ui.Screens
{
    public class Screen : IScreen, IDisposable
    {
        private readonly Dictionary<ScreenType, IScreenMediator> _openScreens = new();
        private IUiFactory _uiFactory;

        [Inject]
        public Screen(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async UniTask Show<TScreenMediator>(ScreenType screenType, CancellationToken cancellationToken)
            where TScreenMediator : IScreenMediator
        {
            CursorOn();

            if (_openScreens.Keys.Contains(screenType))
            {
                Debug.Log("Already opened!");
            }
            else
            {
                var screen = await _uiFactory.InitializeScreen<TScreenMediator>(screenType, cancellationToken);
                _openScreens.Add(screenType, screen);
                await screen.Run(cancellationToken);
            }
        }

        public async UniTask Close(ScreenType screenType, CancellationToken cancellationToken)
        {
            if (_openScreens.Keys.Contains(screenType))
            {
                await _openScreens[screenType].DisposeAsync();
                _openScreens.Remove(screenType);
            }
            else
            {
                Debug.Log("Already closed!");
            }

            if (_openScreens.Count == 0)
            {
                CursorOff();
            }
        }

        public void Dispose()
        {
            _openScreens?.Clear();
        }

        private static void CursorOn()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private static void CursorOff()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}