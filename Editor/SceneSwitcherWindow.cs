#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Build1.UnitySceneSwitcher.Editor
{
    internal sealed class SceneSwitcherWindow : EditorWindow
    {
        private const int Width   = 512;
        private const int Height  = 320;
        private const int Padding = 10;
        
        private void OnGUI()
        {
            var changed = false;
            var enabled = !Application.isPlaying;
            
            if (GUI.enabled != enabled)
            {
                GUI.enabled = enabled;
                changed = true;
            }

            GUILayout.BeginVertical();
            GUILayout.Space(Padding);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(Padding);
            
            GUILayout.BeginVertical();

            if (!SceneSwitcherProcessor.DefaultSceneSelected)
            {
                EditorGUILayout.HelpBox("Select a scene that will open by default when the Unity Editor starts.", MessageType.Info);
                GUILayout.Space(10);    
            }
            
            GUILayout.Label("Current default scene:");
            
            var style = new GUIStyle(EditorStyles.textField)
            {
                alignment = TextAnchor.MiddleLeft
            };
            GUILayout.Label(SceneSwitcherProcessor.DefaultScenePath, style);
            
            GUILayout.Space(10);
            GUILayout.Label("Scenes:");
            
            var _selectedSceneIndex = -1;
            
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var label = scene.path.Replace("Assets/", string.Empty);
                if (GUILayout.Button(label, GUILayout.Height(22)))
                    _selectedSceneIndex = i;
            }

            if (_selectedSceneIndex >= 0)
                SceneSwitcherProcessor.SetDefaultScene(SceneManager.GetSceneAt(_selectedSceneIndex));
            
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();

            if (SceneSwitcherProcessor.DefaultSceneSelected)
            {
                if (GUILayout.Button("Reset", GUILayout.Height(25)))
                    SceneSwitcherProcessor.ResetDefaultScene();
            
                GUILayout.Space(3);
            }
            
            if (GUILayout.Button("Close", GUILayout.Height(25)))
                Close();
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            GUILayout.Space(Padding);
            GUILayout.EndHorizontal();

            GUILayout.Space(Padding);
            GUILayout.EndVertical();
            
            if (changed)
                GUI.enabled = !enabled;
        }
        
        /*
         * Static.
         */

        public static void Open()
        {
            if (HasOpenInstances<SceneSwitcherWindow>())
            {
                FocusWindowIfItsOpen<SceneSwitcherWindow>();
                return;
            }

            var main = EditorGUIUtility.GetMainWindowPosition();
            var centerWidth = (main.width - Width) * 0.5f;
            var centerHeight = (main.height - Height) * 0.5f;
            
            var window = GetWindow<SceneSwitcherWindow>(true, "Scene Switcher", true);
            window.position = new Rect(main.x + centerWidth, main.y + centerHeight, Width, Height);
            window.minSize = new Vector2(Width, Height);
        }
    }
}

#endif