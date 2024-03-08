using UnityEngine;

public class JetpackReset : MonoBehaviour
{
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
