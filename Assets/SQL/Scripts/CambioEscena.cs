using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Librerías necesarias
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public Image fade;

    //Método para cambio de escena
    public void CambioDeEscena(string es)
    {
        //Transición de fade
        fade.CrossFadeAlpha(1, 1, true);
        StartCoroutine(ActivoFade(es));
    }

    //Corutina
    IEnumerator ActivoFade(string e)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(e);
    }
}
