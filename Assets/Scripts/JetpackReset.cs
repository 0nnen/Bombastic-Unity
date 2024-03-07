using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackReset : MonoBehaviour
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
            other.gameObject.GetComponent<MovementController>().CurrentMagic = other.gameObject.GetComponent<MovementController>().MagicSettings.MaxMagic;
            Debug.Log("DESTROY");
            Destroy(gameObject);
        }
    }
}
