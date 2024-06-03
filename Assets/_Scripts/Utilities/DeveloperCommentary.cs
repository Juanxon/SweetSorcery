#if UNITY_EDITOR
using UnityEngine;

public class DeveloperCommentary : MonoBehaviour
{
    [Multiline(6)] // Cambia el número para ajustar el tamaño deseado
    public string DeveloperDescription = "";
    public int textLength = 100;
    public int fontSize = 14;

}
#endif