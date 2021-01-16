using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PlayerAgent))]
    public class PlayerAgentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var agent = (PlayerAgent) target;
            
            GUILayout.Space(20f);
            GUILayout.Label("Observations");
            GUILayout.Space(5f);
            
            Vector2 scaler = Vector2.Scale(PlayerAgent.NormalizeVector, agent._xSide);
            
            // Self
            GUILayout.Label(Vector2.Scale(agent._self.transform.localPosition, scaler).ToString());
            GUILayout.Label(Vector2.Scale(agent._self.velocity.current, scaler).ToString());
            GUILayout.Label(agent._self.currentDashes.ToString());

            // Opponent
            GUILayout.Label(Vector2.Scale(agent._opponent.transform.localPosition, scaler).ToString());
            GUILayout.Label(Vector2.Scale(agent._opponent.velocity.current, scaler).ToString());
            GUILayout.Label(agent._opponent.currentDashes.ToString());
        
            // Ball
            GUILayout.Label(Vector2.Scale(agent._ball.transform.localPosition, scaler).ToString());
            GUILayout.Label(Vector2.Scale(agent._ball.velocity.current, scaler).ToString());
        }
    }
}