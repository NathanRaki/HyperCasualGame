using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, 180f * Time.deltaTime);
        transform.Translate(Vector3.back * gameManager.gameSpeed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gameManager.CollectCoin(1);
            Destroy(gameObject);
        }
        if (other.gameObject.name == "Out")
        {
            Destroy(gameObject);
        }
    }
}
