using UnityEngine;
using System.Collections.Generic;

public class ResetSpawner : MonoBehaviour
{
    public GameObject[] Bonus;

    private Collider areaCollider;
    private GameObject SpawnedObject;
    private List<GameObject> obstacles;

    void Start()
    {
        obstacles = gameObject.GetComponent<GenerateObjects>().Obstacles;
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

        do
        {
            randomPosition.x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
            randomPosition.y = -1f;
            randomPosition.z = Random.Range(collider.bounds.min.z, collider.bounds.max.z);
        } while (InObstacle(randomPosition));

        return randomPosition;
    }

    bool InObstacle(Vector3 position)
    {
        foreach (GameObject obstacle in obstacles)
        {
            Collider obstacleCollider = obstacle.GetComponent<Collider>();
            if (obstacleCollider.bounds.Contains(position))
            {
                Debug.Log("In Obstacle");
                return true;
            }
        }
        return false;
    }
}