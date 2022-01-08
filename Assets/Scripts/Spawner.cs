using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstacle;
    public Transform[] positions;
    public int startDelay = 2;
    public int cooldownDelay = 1;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, cooldownDelay);
    }

    public void SpawnObstacle()
    {
        Instantiate(obstacle, positions[Random.Range(0, positions.Length)].position, Quaternion.identity);
    }
}
