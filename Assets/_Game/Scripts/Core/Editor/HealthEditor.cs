using UnityEditor;
using UnityEngine;

namespace PainterJourney.Core
{
    /// <summary>
    /// Shows the current HP of a Health component in the Inspector, read-only,
    /// so HP is visible without being a number field anyone can type into.
    /// </summary>
    [CustomEditor(typeof(Health))]
    public class HealthEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var health = (Health)target;
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("Current HP", "Shown in Play Mode");
                return;
            }

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.IntField("Current HP", health.Current);
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }
    }
}
