#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Build1.UnitySceneSwitcher.Editor
{
    [InitializeOnLoad]
    internal static class SceneSwitcherProcessor
    {
        private const string ConfigFileName = "build1-scene-switcher.json";

        public static string DefaultScenePath     => _config != null ? _config.defaultScenePath : "None";
        public static bool   DefaultSceneSelected => _config != null && !string.IsNullOrWhiteSpace(_config.defaultScenePath);

        private static string                     _configPath;
        private static SceneSwitcherConfiguration _config;

        static SceneSwitcherProcessor()
        {
            EditorApplication.delayCall += Initialize;
        }

        private static void Initialize()
        {
            _configPath = Path.Combine(Application.dataPath, "Build1", ConfigFileName);
            _config = LoadConfig();

            CheckScene();
        }

        private static void CheckScene()
        {
            if (_config == null || !CheckScenePathValidity(_config.defaultScenePath))
            {
                SceneSwitcherWindow.Open();
                return;
            }

            var currentSceneName = SceneManager.GetActiveScene().name;
            if (!string.IsNullOrWhiteSpace(currentSceneName))
                return;

            EditorApplication.delayCall += () =>
            {
                Debug.Log($"Scene Switcher: Current scene is Untitled. Loading scene: {_config.defaultScenePath}");
                EditorSceneManager.OpenScene(_config.defaultScenePath, OpenSceneMode.Single);
            };
        }

        /*
         * Public.
         */

        public static void SetDefaultScene(Scene scene)
        {
            _config ??= new SceneSwitcherConfiguration();
            _config.defaultScenePath = scene.path;

            var json = JsonUtility.ToJson(_config, true);
            File.WriteAllText(_configPath, json);

            Debug.Log($"Scene Switcher: Default scene set to {_config.defaultScenePath}");

            CheckScene();
        }

        public static void ResetDefaultScene()
        {
            if (!File.Exists(_configPath))
                return;

            File.Delete(_configPath);
            File.Delete(_configPath + ".meta");

            _config = null;

            Debug.Log("Scene Switcher: Default scene reset");
        }

        /*
         * Private.
         */

        private static SceneSwitcherConfiguration LoadConfig()
        {
            if (!File.Exists(_configPath))
                return null;

            var json = File.ReadAllText(_configPath);
            return JsonUtility.FromJson<SceneSwitcherConfiguration>(json);
        }

        private static bool CheckScenePathValidity(string path)
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                if (scenePath == path)
                    return true;
            }            
            return false;
        }
    }
}

#endif