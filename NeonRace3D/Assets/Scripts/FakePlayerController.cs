using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class FakePlayerController : MonoBehaviour
{
    [SerializeField] private GridController Gcontrol;
    // Start is called before the first frame update
    void Start()
    {
        Gcontrol = GetComponentInParent<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        float rand = Random.value;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.parent.name);
            if (hit.transform.parent != null && hit.transform.parent.CompareTag("Obstacle"))
            {
                if(rand >= 0.5f)
                    Gcontrol.TurnRight();
                else
                {
                    Gcontrol.TurnLeft();
                }
            }
        }
    }
}
