using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownButton : MonoBehaviour
{
    public Button button;
    public Image cooldownImage;
    public float cooldownTime = 5f; // Tiempo de enfriamiento en segundos

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        cooldownImage.fillAmount = 1f; // Llenar completamente la imagen al inicio
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = 1 - (cooldownTimer / cooldownTime); // Invertir el llenado

            if (cooldownTimer <= 0)
            {
                isCooldown = false;
                cooldownImage.fillAmount = 1f; // Restaurar llenado completo
                button.interactable = true;
            }
        }
    }

    void OnButtonClick()
    {
        if (!isCooldown)
        {
            button.interactable = false;
            isCooldown = true;
            cooldownTimer = cooldownTime;
        }
    }
}
