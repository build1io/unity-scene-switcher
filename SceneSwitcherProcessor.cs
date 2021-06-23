#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Build1.UnitySceneSwitcher
{
    [InitializeOnLoad]
    internal static class SceneSwitcherProcessor
    {
        static SceneSwitcherProcessor()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (!string.IsNullOrWhiteSpace(currentSceneName)) 
                return;

            EditorApplication.delayCall += () =>
            {
                Debug.Log("Scene Switcher: Current scene is Untitled. Loading Main scene...");
                EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
                Debug.Log("Scene Switcher: Done");
            };
        }
    }
}

#endif