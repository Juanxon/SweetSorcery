using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    public Button myButton;

    public UnitData myUnitData;

    public string columna;

    public Color myColor;

    private void OnEnable()
    {
        myColor = GetComponent<Image>().color;
    }

    public void UpdateGameData()
    {
        ComandosSQLite.Instance.UpdateGameData(columna);
    }
}
