using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    [SerializeField] private Transform target; // R�f�rence � la balle

    private void Update()
    { 
            transform.LookAt(target);
    }
}
