using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Obstacle : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.back * gameManager.gameSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(10f, transform.position, 10f, 1f, ForceMode.Impulse);
            gameManager.PlayerHitByObstacle();
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Out")
        {
            if (gameManager.isPlayerAlive)
                gameManager.PlayerDodgedObstacle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Out")
        {
            Destroy(gameObject);
        }
    }
}
