#if UNITY_EDITOR

using UnityEditor;

namespace Build1.UnitySceneSwitcher.Editor
{
    internal static class SceneSwitcherMenu
    {
        [MenuItem("Tools/Build1/Scene Switcher/Select Default Scene", false, 30)]
        public static void ChangeDefaultScene()
        {
            SceneSwitcherWindow.Open();
        }
    }
}

#endif