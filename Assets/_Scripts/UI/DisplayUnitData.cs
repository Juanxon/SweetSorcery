using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DisplayUnitData : MonoBehaviour
{
    public UnitData unitData;
    [SerializeField] TextMeshProUGUI _costText, _healthText, _damageText, _levelText;


    private void OnEnable()
    {
        unitData.OnValueChange += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        unitData.OnValueChange -= UpdateUI;
    }

    private void UpdateUI()
    {
        _costText.text = unitData.LevelCost.ToString();
        _healthText.text = unitData.Health.ToString();
        _damageText.text = unitData.Damage.ToString();
        _levelText.text = unitData.Level.ToString();
    }
}
