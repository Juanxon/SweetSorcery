using UnityEngine;
using UnityEngine.UI;

public class DisplayUnitHealth : MonoBehaviour
{
    [SerializeField] UnitBehabiourBase _unit;
    [SerializeField] Image _healthBar;

    private int _maxHealth;

    public void SetHealthBar(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public void UpdateHealthBar()
    {
        if (_maxHealth != 0)
        {
            float fillValue = (float)_unit.Health / (float)_maxHealth;
            _healthBar.fillAmount = fillValue;
        }

    }
}

