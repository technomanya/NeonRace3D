using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject grid;
    public GameObject finishLine;
    public Text GameOverText;
    public GameObject ControlButtons;

    [SerializeField] private int gridCount;
    [SerializeField]GameObject[] gridList;
    // Start is called before the first frame update
    void Start()
    {
        GameOverText.gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 0;
        finishLine = GameObject.FindGameObjectWithTag("Finish");
        GameObject beforeGrid = new GameObject();
        float randomAngle = 0f;
        int randomAngleEnum = 0;
        int randomAxis = 0;
        for (int i = 0; i < gridCount; i++)
        {
            randomAngleEnum = Random.Range(0, 8);
            randomAxis = Random.Range(0, 2);
            if (i ==0)
            {
                beforeGrid = Instantiate(grid, Vector3.zero, Quaternion.identity);
                beforeGrid.GetComponentInChildren<SphereCollider>().enabled = false;
            }
            else
            {
                //beforeGrid = Instantiate(grid, beforeGrid.transform.Find("EndTip").position, Quaternion.Euler(new Vector3(randomAngle, 0, 0)));
                beforeGrid = Instantiate(grid, beforeGrid.transform.Find("EndTip").position, beforeGrid.transform.rotation);
                switch (randomAngleEnum)
                {
                    case 0:
                        randomAngle = 30f;
                        break;
                    case 1:
                        randomAngle = 45f;
                        break;
                    case 2:
                        randomAngle = 60f;
                        break;
                    case 3:
                        randomAngle = 75f;
                        break;
                    case 4:
                        randomAngle = -30f;
                        break;
                    case 5:
                        randomAngle = -45f;
                        break;
                    case 6:
                        randomAngle = -60f;
                        break;
                    case 7:
                        randomAngle = -75f;
                        break;
                }
                if (randomAxis == 0)
                {
                    beforeGrid.transform.Rotate(Vector3.up, randomAngle);
                }
                else
                {
                    beforeGrid.transform.Rotate(Vector3.right, randomAngle);
                }
            }
            beforeGrid.SetActive(true);
        }
        gridList = GameObject.FindGameObjectsWithTag("Grid");
        foreach (var grid in gridList)
        {
            grid.transform.eulerAngles = new Vector3(grid.transform.eulerAngles.x,grid.transform.eulerAngles.y, 0);
        }

        finishLine.transform.position = gridList[gridList.Length - 1].transform.Find("EndTip").position;
    }

    public void GameOver(string playerName)
    {
        GameOverText.text = playerName + " WIN";
        GameOverText.gameObject.transform.parent.gameObject.SetActive(true);
        ControlButtons.SetActive(false);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
    }

    public void Begin()
    {
        Time.timeScale = 1;
    }
}
