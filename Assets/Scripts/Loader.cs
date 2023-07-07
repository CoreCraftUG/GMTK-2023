using UnityEngine.SceneManagement;

namespace JamCraft.GMTK2023.Code
{
    public static class Loader
    {
        public enum Scene
        {
            mainmenu_scene,
            game_scene,
            loading_scene
        }

        private static Scene _targetScene;

        public static void Load(Scene targetScene)
        {
            // Set target scene to load and load the loading scene beforehand.
            _targetScene = targetScene;

            SceneManager.LoadScene(Scene.loading_scene.ToString());
        }

        public static void LoaderCallback()
        {
            // Load actual game scene on first update call.
            SceneManager.LoadScene(_targetScene.ToString());
        }
    }
}