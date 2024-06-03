using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DisplayResource : MonoBehaviour
{
    [SerializeField] ResourceData _data;
    [SerializeField] Image _slider;
    [SerializeField] TextMeshProUGUI _text;

    [SerializeField] bool _hasSlider = true;

    private void OnEnable()
    {
        UpdateCanvas();
        _data.onValueChange += UpdateCanvas;
    }

    private void OnDisable()
    {
        _data.onValueChange -= UpdateCanvas;
    }

    void UpdateCanvas()
    {
        if (_hasSlider)
        {
            _slider.fillAmount = _data.ValueFillAmount;
            _text.text = _data.Value.ToString();
        }
        else
        {
            _text.text = _data.Value.ToString();
        }
    }
}
