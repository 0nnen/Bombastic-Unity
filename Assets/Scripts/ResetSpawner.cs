using UnityEngine;

public class ResetSpawner : MonoBehaviour
{
    public GameObject[] Bonus;

    private Collider areaCollider;
    private GameObject SpawnedObject;

    void Start()
    {
        areaCollider = gameObject.GetComponent<Collider>();
        SpawnRandomObject();
    }

    void Update()
    {
       if(SpawnedObject == null) {
            Debug.Log("Respawn Reset");
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        Vector3 randomPosition = GetRandomPositionInsideCollider(areaCollider);

        GameObject randomObject = Bonus[Random.Range(0, Bonus.Length)];

        SpawnedObject = Instantiate(randomObject, randomPosition, Quaternion.identity);
    }

    Vector3 GetRandomPositionInsideCollider(Collider collider)
    {
        Vector3 randomPosition;

        // Generate a random point inside the collider's bounds
        randomPosition.x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
        randomPosition.y = -1f;
        randomPosition.z = Random.Range(collider.bounds.min.z, collider.bounds.max.z);

        return randomPosition;
    }

}
