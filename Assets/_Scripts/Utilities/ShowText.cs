using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShowText : MonoBehaviour
{
    public TMP_Text textoAMostrar;
    public string[] textoArray;
    public UnityEventContainer[] textEvent;
    public UnityEvent onUltimoTextoMostrado;
    private int indiceTextoActual;

    void Start()
    {
        indiceTextoActual = 0;
        ActualizarTexto();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            MostrarSiguienteTexto();
        }
    }

    void MostrarSiguienteTexto()
    {
        indiceTextoActual++;

        if (indiceTextoActual >= textoArray.Length)
        {
            indiceTextoActual = 0;
            onUltimoTextoMostrado.Invoke(); // Invocar el evento cuando se muestra el último texto
        }

        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        textoAMostrar.text = textoArray[indiceTextoActual];
        textEvent[indiceTextoActual].myEvent.Invoke();
        //textEvent[indiceTextoActual].Invoke();
    }
}

[System.Serializable]
public struct UnityEventContainer
{
    public UnityEvent myEvent;
}
