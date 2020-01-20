using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities
{
    [System.Serializable]
    public class UnityScene
    {
        [SerializeField] private Object sceneAsset = null;
        [SerializeField] private string sceneName = "";

        public string SceneName
        {
            get { return sceneName; }
        }

        public static implicit operator string(UnityScene sceneField)
        {
            return sceneField.SceneName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UnityScene))]
    public class UnityScenePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            var asset = property.FindPropertyRelative("sceneAsset");
            var name = property.FindPropertyRelative("sceneName");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (asset != null)
            {
                EditorGUI.BeginChangeCheck();
                var value = EditorGUI.ObjectField(position, asset.objectReferenceValue, typeof(SceneAsset), false);

                if (EditorGUI.EndChangeCheck())
                {
                    asset.objectReferenceValue = value;
                    if (asset.objectReferenceValue != null)
                    {
                        var path = AssetDatabase.GetAssetPath(asset.objectReferenceValue);
                        var assetsIndex = path.IndexOf("Assets", StringComparison.Ordinal) + 7;
                        var extensionIndex = path.LastIndexOf(".unity", StringComparison.Ordinal);
                        path = path.Substring(assetsIndex, extensionIndex - assetsIndex);
                        name.stringValue = path;
                    }
                }
            }
            EditorGUI.EndProperty();
        }
    }
#endif
}
