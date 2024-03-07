using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<MovementController>().CurrentStamina = other.gameObject.GetComponent<MovementController>().StaminaSettings.MaxStamina;
            Destroy(gameObject);        
        }
    }
}
