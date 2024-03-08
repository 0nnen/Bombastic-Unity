using UnityEngine;
using System.Collections.Generic;

public class GenerateObjects : MonoBehaviour
{
    public GameObject objectToGenerate;
    public int numberOfObjects = 10;
    public Vector2 scaleRange = new Vector2(0.5f, 2.0f);

    private List<GameObject> obstacles = new List<GameObject>();

    public List<GameObject> Obstacles
    {
        get { return obstacles; }
    }

    void Start()
    {
        GenerateObjectsInArea();
    }

    void GenerateObjectsInArea()
    {
        Collider areaCollider = gameObject.GetComponent<Collider>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPosition = GetRandomPositionInsideCollider(areaCollider);

            GameObject generatedObject = Instantiate(objectToGenerate, randomPosition, Quaternion.identity);

            generatedObject.transform.localScale = new Vector3(Random.Range(scaleRange.x, scaleRange.y), Random.Range(scaleRange.x, scaleRange.y), Random.Range(scaleRange.x, scaleRange.y));

            generatedObject.transform.rotation = Random.rotation;

            obstacles.Add(generatedObject);

        }
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
                return true;
            }
        }
        return false;
    }
}
