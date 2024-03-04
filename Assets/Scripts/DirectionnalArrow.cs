using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    [SerializeField] private Transform target; // Référence à la balle

    private void Update()
    { 
            transform.LookAt(target);
    }
}
