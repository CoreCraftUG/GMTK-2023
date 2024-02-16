using UnityEngine.SceneManagement;

namespace JamCraft.GMTK2023.Code
{
    public static class Loader
    {
        private static string _targetSceneName;

        public static void Load(string targetSceneName)
        {
            // Set target scene to load and load the loading scene beforehand.
            _targetSceneName = targetSceneName;

            SceneManager.LoadScene("loading_scene");
        }

        public static void LoaderCallback()
        {
            // Load actual game scene on first update call.
            SceneManager.LoadScene(_targetSceneName);
        }
    }
}