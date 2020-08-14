using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Grid Objects")]
    public GameObject[] GridPrefabs;
    public GameObject grid;
    public GameObject finishLine;
    public GameObject[] gridList;
    [SerializeField] private GridController Gcontroller;

    [SerializeField] private int gridCount;
    public GameObject ControlButtons;
    [Space(20)]

    [Header("Game UI Objects")]
    public GameObject StartImage;
    public GameObject InGameImage;
    public GameObject GameOverObj;
    public Text CoinText;
    public Text ScoreText;
    [SerializeField] private Image youWin;
    [SerializeField] private Image youLose;
    [Space(20)]

    [Header("Game Attributes")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int _positivePoint = 0;
    [SerializeField] private int _negativePoint = 0;
    [SerializeField] private int _gameSeconds = 0;
    public int Coins = 0;
    [SerializeField] private PlayerControllerWaypoint playerControllerWP;
    private bool _isLevelBegin = false;

    

    public enum  SceneIndexConstant
    {
        MainScene = 0,
        AllSidedScene = 1,
        FourSidedScene = 2
        
    }

    public enum PointSystem
    {
        PositivePoint,
        NegativePoint,
        Seconds
    }

    void Awake()
    {
        //CoinText.gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 0;

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            foreach (var gridPref in GridPrefabs)
            {
                if(gridPref.activeInHierarchy)
                    gridPref.SetActive(false);
            }
            int randGrid = Random.Range(0, GridPrefabs.Length-1);
            GridPrefabs[randGrid].SetActive(true);
            gridList = GameObject.FindGameObjectsWithTag("Grid");
        }
        else if(SceneManager.GetActiveScene().name == "SceneMaker")
        {
            finishLine = GameObject.FindGameObjectWithTag("Finish");
            GameObject beforeGrid = new GameObject();
            float randomAngle = 0f;
            int randomAngleEnum = 0;
            int randomAxis = 0;
            for (int i = 0; i < gridCount; i++)
            {
                randomAngleEnum = Random.Range(0, 8);
                randomAxis = Random.Range(0, 2);
                if (i == 0)
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
                grid.transform.eulerAngles = new Vector3(grid.transform.eulerAngles.x, grid.transform.eulerAngles.y, 0);
            }

            finishLine.transform.position = gridList[gridList.Length - 1].transform.Find("EndTip").position;
            finishLine.transform.rotation = gridList[gridList.Length - 1].transform.rotation;
        }
        //GameOverObj.gameObject.transform.parent.gameObject.SetActive(false);

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerControllerWP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerWaypoint>();
        GameAnalyticsSDK.GameAnalytics.Initialize();
    }

    void Update()
    {
    #if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene((int)SceneIndexConstant.MainScene);
#endif
        if (_isLevelBegin == false)
        {
            float mouseXdif = playerControllerWP.mouseX;
            if (Input.GetMouseButton(0) && mouseXdif == 0.0f)
            {
                Begin();
                StartImage.SetActive(false);
            }
        }
    }

    public void GameOver(string playerName)
    {
        if(playerName == "Player")
        {
            youLose.gameObject.SetActive(false);
            youWin.gameObject.SetActive(true);
        }
        else
        {
            youWin.gameObject.SetActive(false);
            youLose.gameObject.SetActive(true);
        }
        PointCalculator(PointSystem.Seconds, (int)Time.realtimeSinceStartup);
        var coin = Mathf.Clamp((int)((_positivePoint - _negativePoint) * 60 / _gameSeconds), 0, Mathf.Infinity);
        var score = coin * 7;
        coin += Coins * 3;

        CoinText.text = coin.ToString();
        ScoreText.text = score.ToString();

        //CoinText.gameObject.transform.parent.gameObject.SetActive(true);

        GameOverObj.SetActive(true);
        Time.timeScale = 0;
        audioSource.Stop();
    }

    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
        audioSource.Play();
        if(_isLevelBegin == true)
            _isLevelBegin = false;
    }

    public void Begin()
    {
        Time.timeScale = 1;
        //PointCalculator(PointSystem.Seconds, (int)Time.realtimeSinceStartup);
        Gcontroller._canTurn = true;
        audioSource.Play();
        _isLevelBegin = true;
        InGameImage.SetActive(true);
    }

    public void PauseContinue(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void PointCalculator(PointSystem type, int points)
    {
        switch (type)
        {
            case PointSystem.PositivePoint:
                _positivePoint += points;
                break;
            case PointSystem.NegativePoint:
                _negativePoint += points;
                break;
            case PointSystem.Seconds:
                _gameSeconds = Mathf.Clamp(points - _gameSeconds, 1 , 60);
                break;
        }
    }
}
