using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject poolObj;
    public float distanceMin;
    public float distanceMax;

    [SerializeField] private int objCount = 0;
    [SerializeField] private List<GameObject> poolObjectsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MakeObjects(poolObj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeObjects(GameObject obj)
    {
        GameObject tempObj;
        for (int i = 0; i < objCount; i++)
        {
            tempObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
            tempObj.transform.SetParent(gameObject.transform);
            tempObj.SetActive(true);
            poolObjectsList.Add(tempObj);
        }
        MoveObjects(poolObjectsList);
    }

    void MoveObjects(List<GameObject> objList)
    {
        float posZ = 0;
        float posX = 0;
        float posY = 0;
        int posXYodds = 0;
        foreach (var obj in objList)
        {
            posZ += Random.Range(distanceMin, distanceMax);
            posXYodds = Random.Range(0, 4);
            switch (posXYodds)
            {
                case 0:
                    posX = 1.5f;
                    posY = 0;
                    break;
                case 1:
                    posX = -1.5f;
                    posY = 0;
                    break;
                case 2:
                    posX = 0;
                    posY = 1.5f;
                    break;
                case 3:
                    posX = 0;
                    posY = -1.5f;
                    break;
            }

            obj.transform.localPosition = new Vector3(posX,posY,posZ);
            obj.transform.rotation = obj.transform.parent.rotation;
        }
    }
}
