using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLButtonHelper : MonoBehaviour
{
    public void UpdateGameData(string columna)
    {
        ComandosSQLite.Instance.UpdateGameData(columna);
    }
}
