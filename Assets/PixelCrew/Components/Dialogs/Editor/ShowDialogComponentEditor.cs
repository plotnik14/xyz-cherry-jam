using PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.Components.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        
        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_useLocalization"));
            
            if (_modeProperty.TryGetEnum(out ShowDialogComponent.Mode mode))
            {
                switch (mode)
                {
                    case ShowDialogComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_boundDialog"));
                        break;
                    case ShowDialogComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_externalDialog"));
                        break;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}