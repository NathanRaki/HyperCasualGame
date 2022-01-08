using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int coins = 0;
    private float distance = 0f;

    private TextMeshProUGUI distanceText;

    [Header("Game Variables")]
    public float gameSpeed = 10f;

    [Header("Particles")]
    public GameObject dodgeParticles;
    public GameObject deathParticles;
    public GameObject collectParticles;

    private GameObject player;
    private Transform bonusParticlesPosition;
    private ParticleSystem speedParticles;

    public bool isPlayerAlive = true;

    private void Start()
    {
        try { bonusParticlesPosition = GameObject.Find("Bonus Particles Position").transform; }
        catch (Exception e) { Debug.Log(e.Message); }

        try { player = GameObject.Find("Player"); }
        catch (Exception e) { Debug.Log(e.Message); }

        try { distanceText = GameObject.Find("Distance Text").GetComponent<TextMeshProUGUI>(); }
        catch (Exception e) { Debug.Log(e.Message); }

        try { speedParticles = GameObject.Find("Speed Particles").GetComponent<ParticleSystem>(); }
        catch (Exception e) { Debug.Log(e.Message); }
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            distance += gameSpeed;
            float roundedDistance = (int)(distance * 10.0f) / 10.0f;
            distanceText.text = roundedDistance.ToString() + "m";
            
            gameSpeed += Time.deltaTime / 1000.0f;
            var main = speedParticles.main;
            main.startSpeed = gameSpeed;
        }
        
    }

    public void CollectCoin(int value)
    {
        Instantiate(collectParticles, bonusParticlesPosition.position, Quaternion.identity);
        coins += value;
    }

    public void PlayerHitByObstacle()
    {
        if (player)
        {
            Instantiate(deathParticles, player.transform.position, Quaternion.identity);
            iTween.PunchScale(player, new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
            isPlayerAlive = false;
            Destroy(player, 0.5f);
            Invoke("ReloadLevel", 3);
        }
    }

    public void PlayerDodgedObstacle()
    {
        if (bonusParticlesPosition)
        {
            Instantiate(dodgeParticles, bonusParticlesPosition.position, Quaternion.identity);
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetBonusParticlesPosition()
    {
        bonusParticlesPosition.Translate(new Vector3(player.transform.position.x - bonusParticlesPosition.position.x, 0f, 0f));
    }
}
