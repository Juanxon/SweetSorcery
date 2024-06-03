using TMPro;
using UnityEngine;

public class DisplayUnitCost : MonoBehaviour
{
    [SerializeField] UnitData _unitData;
    [SerializeField] TMP_Text _textMeshPro;
    private void Start()
    {
        _textMeshPro.text = _unitData.SpawnCost.ToString();
    }
}
