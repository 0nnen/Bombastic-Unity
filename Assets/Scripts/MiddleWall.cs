using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleWall : MonoBehaviour
{
    public GameObject Barriere;
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
        if (other.gameObject.CompareTag("BALL"))
        {
            Debug.Log("IN");
            Collider collider = Barriere.GetComponent<Collider>();
            collider.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("out");
        if (other.gameObject.CompareTag("BALL"))
        {
            Barriere.GetComponent<Collider>().enabled = true;
        }
    }
}
