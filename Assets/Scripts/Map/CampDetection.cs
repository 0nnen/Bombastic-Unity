using UnityEngine;

public class CampDetection : MonoBehaviour
{
    public int camp;

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            Debug.Log("Ball entered camp: " + camp);
            other.gameObject.GetComponent<BallManager>().SetCamp(camp);
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            Debug.Log("Ball is still in camp: " + camp);
            other.gameObject.GetComponent<BallManager>().SetCamp(camp);
        }
    }

/*    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            if (camp == 1)
            {
                Debug.Log("Ball left camp 1");
            }
            else if (camp == 2)
            {
                Debug.Log("Ball left camp 2");
            }
        }
    }*/
}
