using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GameObject Grid;
    private GameObject oldGrid;
    [SerializeField] private GameObject player;

    public float speed;

    public float rotDuration;

    public bool _canTurn;

    public void ChangeBaseGrid(GameObject newGrid)
    {
        oldGrid = Grid;
        Grid = newGrid;
        Invoke("DestroyGrid",2);
    }

    // Start is called before the first frame update
    void Start()
    {
        _canTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TurnRight();
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            TurnLeft();
        }
    }

    IEnumerator Rotate(GameObject rotateObj, Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = rotateObj.transform.rotation;
        Quaternion to = rotateObj.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            rotateObj.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rotateObj.transform.rotation = to;
        _canTurn = true;
    }

    public void TurnRight()
    {
        if (_canTurn)
        {
            _canTurn = false;
            StartCoroutine(Rotate(gameObject, Vector3.forward, -90, rotDuration));
        }
        
    }

    public void TurnLeft()
    {
        if (_canTurn)
        {
            _canTurn = false;
            StartCoroutine(Rotate(gameObject, Vector3.forward, 90, rotDuration));
        }
        
    }

    void DestroyGrid()
    {
        Destroy(oldGrid);
    }
}
