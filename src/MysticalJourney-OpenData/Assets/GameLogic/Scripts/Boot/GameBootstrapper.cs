using StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic.Boot
{
    public class GameBootstrapper : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
                if (SceneManager.GetSceneAt(sceneIndex).name == Constants.BootSceneName)
                    return;

            SceneManager.LoadScene(Constants.BootSceneName, LoadSceneMode.Single);
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}