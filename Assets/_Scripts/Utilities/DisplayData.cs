using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DisplayData : MonoBehaviour
{
    public Data data;
    [SerializeField] TextMeshProUGUI _text;

    private void OnEnable()
    {
        data.OnValueChange += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        data.OnValueChange -= UpdateUI;
    }

    private void UpdateUI()
    {
        _text.text = data.Value.ToString();
    }
}
