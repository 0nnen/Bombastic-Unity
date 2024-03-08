using UnityEngine;

public class CampDetection : MonoBehaviour
{
    public int camp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            Debug.Log("Ball in camp :" + camp);
            other.gameObject.GetComponent<BallManager>().SetCamp(camp);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            if (camp == 1)
            {
                Debug.Log("Ball in camp : 2");
                other.gameObject.GetComponent<BallManager>().SetCamp(2);
            }
            else if (camp == 2)
            {
                Debug.Log("Ball in camp : 1");
                other.gameObject.GetComponent<BallManager>().SetCamp(1);
            }

        }
    }
}