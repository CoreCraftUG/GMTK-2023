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
            _targetScene = targetScene;

            SceneManager.LoadScene(Scene.loading_scene.ToString());

            
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(_targetScene.ToString());
        }
    }
}