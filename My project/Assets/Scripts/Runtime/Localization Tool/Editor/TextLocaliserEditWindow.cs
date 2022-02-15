using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace RogueLike.Localization
{
    public class TextLocaliserEditWindow : EditorWindow
    {
        public Language language;
        public string key;
        public string value;


        [MenuItem("Rogue Like Prototype/TextLocaliserEditWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<TextLocaliserEditWindow>();
            window.titleContent = new GUIContent("TextLocaliserEditWindow");
            window.Show();
        }
    
        private void OnGUI()
        {
            key = EditorGUILayout.TextField("Key :", key);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value :", GUILayout.MaxWidth(50));

            EditorStyles.textArea.wordWrap = true;
            value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("Add"))
            {
                if(LocalizationSystem.GetLocalisedValue(key) != string.Empty)
                {
                    LocalizationSystem.Replace(key, value);
                }
                else
                {
                    LocalizationSystem.Add(key, value);
                }
            }

            minSize = new Vector2(460, 250);
            maxSize = minSize;

        }

        public static void Open(string key)
        {
            TextLocaliserEditWindow window = new TextLocaliserEditWindow();
            window.titleContent = new GUIContent("Localiser Window");
            window.ShowUtility();
            window.key = key;
        }
    }

    public class TextLocaliserSearchWindow : EditorWindow
    {
        public string value;
        public Vector2 scroll;

        public Dictionary<string, string> dictionary;

        public static void Open()
        {
            TextLocaliserSearchWindow window = new TextLocaliserSearchWindow();
            window.titleContent = new GUIContent("Localisation Search");

            Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
            window.ShowAsDropDown(r, new Vector2(500, 300));
        }

        private void OnEnable()
        {
            dictionary = LocalizationSystem.GetDictionaryForEditor();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search :", EditorStyles.boldLabel);
            value = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();

            GetSearchResults();
        }

        private void GetSearchResults()
        {
            if(value == null) { return; }

            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (KeyValuePair<string, string> element in dictionary)
            {
                if(element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal("box");
                    Texture closeIcon = (Texture)Resources.Load("close");

                    GUIContent content = new GUIContent(closeIcon);

                    if(GUILayout.Button((content), GUILayout.MaxHeight(20), GUILayout.MaxWidth(20)))
                    {
                        if(EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localusation, are you sure?", "Do it"))
                        {
                            LocalizationSystem.Remove(element.Key);
                            AssetDatabase.Refresh();
                            LocalizationSystem.Init();
                            dictionary = LocalizationSystem.GetDictionaryForEditor();
                        }
                    }

                    EditorGUILayout.TextField(element.Key);
                    EditorGUILayout.LabelField(element.Value);
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}