using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Player))]
    public class PlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var player = (Player) target;
            
            GUILayout.Space(20f);
            GUILayout.Label("State: " + player.State);
        }
    }
}