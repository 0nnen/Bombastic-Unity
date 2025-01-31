using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target != null)
        {
            // Calculer la direction vers la cible
            Vector3 direction = target.transform.position - transform.position;

            // Creer une rotation basee sur la direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Transformer la rotation pour s'adapter a votre orientation specifique
            Quaternion finalRotation = Quaternion.Euler(targetRotation.eulerAngles.z, targetRotation.eulerAngles.y + 90, targetRotation.eulerAngles.x);

            // Appliquer la rotation
            transform.rotation = finalRotation;
        }
        else
        {
            Debug.LogWarning("Aucune cible n'est d?finie pour la fl?che.");
        }
    }
}