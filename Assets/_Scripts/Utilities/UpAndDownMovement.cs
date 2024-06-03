using UnityEngine;

public class UpAndDownMovement : MonoBehaviour
{
    public float amplitude = 1f; // Amplitud del movimiento
    public float speed = 1f;     // Velocidad del movimiento

    private Vector3 startPosition;

    void Start()
    {
        // Guarda la posici�n inicial del objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcula el desplazamiento vertical basado en el tiempo
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;

        // Actualiza la posici�n del objeto con el desplazamiento
        transform.position = startPosition + Vector3.up * yOffset;
    }
}