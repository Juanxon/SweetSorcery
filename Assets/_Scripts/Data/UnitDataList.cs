using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/UnitDataList")]
public class UnitDataList : ScriptableObject
{
    public List<UnitData> units;
}


#if UNITY_EDITOR
public class UnitComparisonWindow : EditorWindow
{
    private UnitDataList unitDataList;
    private Vector2 scrollPosition;

    [MenuItem("Window/Unit Comparison")]
    public static void ShowWindow()
    {
        GetWindow<UnitComparisonWindow>("Unit Comparison");

    }

    private void OnGUI()
    {
        GUILayout.Label("Unit Comparison", EditorStyles.boldLabel);

        unitDataList = (UnitDataList)EditorGUILayout.ObjectField("Unit Data List", unitDataList, typeof(UnitDataList), false);

        if (unitDataList != null)
        {
            foreach (var unit in unitDataList.units)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Unit: " + unit.name, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Level Cost", unit.LevelCost.ToString());
                EditorGUILayout.LabelField("Level", unit.Level.ToString());
                EditorGUILayout.LabelField("Health", unit.Health.ToString());
                EditorGUILayout.LabelField("Damage", unit.Damage.ToString());
                EditorGUILayout.LabelField("Range", unit.Range.ToString());
                EditorGUILayout.LabelField("FireRate", unit.FireRate.ToString());
                EditorGUILayout.LabelField("Speed", unit.Speed.ToString());
                EditorGUILayout.LabelField("Power", unit.Power.ToString());
                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
            }
        }
    }
}
#endif
