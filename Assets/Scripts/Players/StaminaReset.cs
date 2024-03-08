using UnityEngine;

public class StaminaReset : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<MovementController>().CurrentStamina = other.gameObject.GetComponent<MovementController>().StaminaSettings.MaxStamina;
            Destroy(gameObject);        
        }
    }
}
