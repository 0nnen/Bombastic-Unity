using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATHBars : MonoBehaviour
{
    [SerializeField] private Image staminaBarFill;
    [SerializeField] private Slider magicBarFill;
    [SerializeField] private MovementController playerMovementController;

    private float maxStamina;
    private float maxMagic;

    void Start()
    {
        // Assurez-vous que le contrôleur de mouvement est assigné
        if (playerMovementController != null)
        {
            maxStamina = playerMovementController.StaminaSettings.MaxStamina;
            maxMagic = playerMovementController.MagicSettings.MaxMagic;
        }
    }

    void Update()
    {
        if (playerMovementController != null)
        {
            if (staminaBarFill != null)
            {
                // Mettez à jour l'affichage de la barre de stamina en fonction de la stamina actuelle
                staminaBarFill.fillAmount = playerMovementController.CurrentStamina / maxStamina;
            }

            if (magicBarFill != null)
            {
                // Mettez à jour l'affichage de la barre de magie en fonction de la magie actuelle
                magicBarFill.value = playerMovementController.CurrentMagic / maxMagic;
            }
        }
    }
}
