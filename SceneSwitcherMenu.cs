#if UNITY_EDITOR

using UnityEditor;

namespace Build1.UnitySceneSwitcher
{
    internal static class SceneSwitcherMenu
    {
        [MenuItem("Tools/Build1/Scene Switcher/Select Default Scene", false, 20)]
        public static void ChangeDefaultScene()
        {
            SceneSwitcherWindow.Open();
        }
    }
}

#endif