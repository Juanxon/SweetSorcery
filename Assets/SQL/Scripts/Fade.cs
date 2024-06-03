using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Para acceder a la UI de Unity
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    //Para acceder a la imagen de Fade
    public Image fade;

    // Start is called before the first frame update
    void Start()
    {
        //Transición de fade
        fade.CrossFadeAlpha(0, 1, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
