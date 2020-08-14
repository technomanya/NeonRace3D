using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerImageController : MonoBehaviour
{
    private float speed;
    private ParticleSystem powerFX;
    private ParticleSystem obstacleFX;
    private GridController gridCon;
    private float endCounterSec;
    private bool _isGameOver = false;
    private AudioSource[] audios;

    public PlayerController PlayerController;
    public PlayerControllerWaypoint PlayerControllerWP;
    public GameManager GM;
    public float maxSpeed = 20.0f;
    public float minSpeed = 5.0f;
    public Camera MainCamera;

    void Start()
    {
        PlayerController = GetComponentInParent<PlayerController>();
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (gameObject.GetComponentInParent<PlayerControllerWaypoint>())
        {
            PlayerControllerWP = GetComponentInParent<PlayerControllerWaypoint>();
            speed = gameObject.GetComponentInParent<PlayerControllerWaypoint>().PlayerSpeed;
        }
        else
        {
            PlayerController = GetComponentInParent<PlayerController>();
            speed = PlayerController.speed;
        }

        foreach (var effectFX in GetComponentsInChildren<ParticleSystem>())
        {
            if (effectFX.CompareTag("PowerFX"))
                powerFX = effectFX;
            else if (effectFX.CompareTag("ObstacleFX"))
                obstacleFX = effectFX;
            else
                Debug.LogError("There is no power or obstacle effects !");
        }

        audios = GetComponents<AudioSource>();

        gridCon = GetComponentInParent<GridController>();

        MainCamera = Camera.main;
    }

    void Update()
    {
        endCounterSec += Time.deltaTime;
        int seconds = (int)endCounterSec % 60;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Obstacle"))
        {
            Debug.Log("ObstacleHit");
            if (speed >= minSpeed)
            {
                speed -= speed * 0.3f;
                PlayerControllerWP.PlayerSpeed = speed;
            }
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<Renderer>().enabled = false;
            obstacleFX.Play();
            audios[0].Play();
            gridCon.tronRunning.SetTrigger("Stumble");

            GM.PointCalculator(GameManager.PointSystem.NegativePoint, 5);
            //gameObject.GetComponent<GridController>().speed -= gameObject.GetComponent<GridController>().speed * 0.2f;
        }
        else if (other.transform.parent.CompareTag("Power"))
        {
            Debug.Log("PowerHit");
            if (speed <= maxSpeed)
            {
                speed += speed;
                PlayerControllerWP.PlayerSpeed = speed;
            }
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<Renderer>().enabled = false;
            powerFX.Play();
            audios[1].Play();
            gridCon.tronRunning.SetTrigger("Sprint");

            GM.PointCalculator(GameManager.PointSystem.PositivePoint, 10);
            //gameObject.GetComponent<GridController>().speed *= 2;
        }
        else if (other.transform.CompareTag("CoinObj"))
        {
            GM.Coins++;
            other.GetComponent<Renderer>().enabled = false;
        }
    }
}
