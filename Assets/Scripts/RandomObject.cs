using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] myObjects;
    public float spawnInterval = 10.0f;

    private float spawnLimitXLeft = -38;
    private float spawnLimitXRight = 0;
    private float spawnPosY = -1;

    private float startDelay = 5.0f;

    void Start()
    {
        Invoke("SpawnRandomObject", startDelay);
        }

    void SpawnRandomObject()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), spawnPosY, Random.Range(spawnLimitXLeft, spawnLimitXRight));

        int randomIndex = Random.Range(0, myObjects.Length);

        GameObject newObj = Instantiate(myObjects[randomIndex], randomSpawnPosition, Quaternion.identity);

        float nextSpawnInterval = Random.Range(3.0f, 5.0f);

        Debug.Log("Next object will spawn in " + nextSpawnInterval + " seconds.");
        Invoke("SpawnRandomObject", nextSpawnInterval);
    }

    // G�rer les collisions avec les objets de r�cup�ration de stamina et de magic
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name); // Afficher le nom de l'objet avec lequel la collision est d�tect�e

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!"); // Afficher un message si le joueur est d�tect�

            MovementController movementController = other.GetComponent<MovementController>(); // R�cup�rer le composant MovementController
            if (movementController != null)
            {
                if (other.CompareTag("StaminaObjectBall"))
                {
                    Debug.Log("Stamina object detected!"); // Afficher un message si l'objet de r�cup�ration de stamina est d�tect�

                    movementController.RestoreStaminaToMax(); // Restaurer la stamina au maximum
                }
                else if (other.CompareTag("JetPackObjectBall"))
                {
                    Debug.Log("Jetpack object detected!"); // Afficher un message si l'objet de r�cup�ration de jetpack est d�tect�

                    movementController.RestoreMagicToMax(); // Restaurer la magie au maximum
                }

                Destroy(other.gameObject); // D�truire l'objet avec lequel nous avons collision
            }
        }
    }


}
