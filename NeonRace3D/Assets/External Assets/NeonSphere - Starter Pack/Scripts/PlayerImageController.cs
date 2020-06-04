using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerImageController : MonoBehaviour
{
    private float speed;
    public PlayerController PlayerController;

    void Start()
    {
        PlayerController = GetComponentInParent<PlayerController>();
        speed = PlayerController.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collide");
        if (other.transform.parent.CompareTag("Obstacle") && speed > 5.0f)
        {
            //Destroy(other.gameObject);
            speed -= speed * 0.2f;
            PlayerController.speed = speed;
            //gameObject.GetComponent<GridController>().speed -= gameObject.GetComponent<GridController>().speed * 0.2f;
        }
        else if (other.transform.parent.CompareTag("Power") && speed < 20.0f)
        {
            //Destroy(other.gameObject);
            speed += speed * 0.5f;
            PlayerController.speed = speed;
            //gameObject.GetComponent<GridController>().speed *= 2;

        }
    }
}
