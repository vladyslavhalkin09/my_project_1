using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData), editorForChildClasses: true)]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyData data = (EnemyData)target;

        float statBonus = data.mainStat * data.statMultiplier;
        float minTotal = data.minDamage + statBonus;
        float maxTotal = data.maxDamage + statBonus;

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            $"Preview Attack Damage: {minTotal:F1} – {maxTotal:F1}"
            + (statBonus > 0f ? $"  (base {data.minDamage:F1}–{data.maxDamage:F1} + {statBonus:F1} from MainStat)" : ""),
            MessageType.Info);
    }
}