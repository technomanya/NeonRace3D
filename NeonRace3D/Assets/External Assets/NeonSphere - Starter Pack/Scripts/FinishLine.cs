using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject GM;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController");
    }
    private void OnTriggerEnter(Collider other)
    {
        GM.GetComponent<GameManager>().GameOver(other.name);
    }
}
