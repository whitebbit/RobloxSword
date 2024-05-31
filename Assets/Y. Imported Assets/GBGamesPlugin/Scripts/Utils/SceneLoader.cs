using UnityEngine.SceneManagement;

namespace GBGamesPlugin
{
    public static class SceneLoader
    {
        public static void LoadScene(string name)
        {
            if (GBGames.instance.settings.gameLoadingCallbacksOnSceneLoading)
                GBGames.InGameLoadingStarted();
            
            SceneManager.LoadScene(name);
        }

        public static void LoadScene(int id)
        {
            if (GBGames.instance.settings.gameLoadingCallbacksOnSceneLoading)
                GBGames.InGameLoadingStarted();
            
            SceneManager.LoadScene(id);
        }
    }
}