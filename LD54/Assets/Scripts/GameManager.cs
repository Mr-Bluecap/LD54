using System;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField]
    float timeInSeconds = 120f;

    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    GameObject timerPanel;
    
    [SerializeField]
    GameObject scorePanel;
    
    [SerializeField]
    GameObject submitButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HideGeneralUI();
    }

    public void StartGame()
    {
        GameOverManager.Instance.HideDisplay();
        ScoreManager.Instance.ResetScore();
        ShowGeneralUI();
        CreateNewSequence();
        StartLevelTimer();
    }

    public void CreateNewSequence()
    {
        RoomLineManager.Instance.DestroyAllLines();
        MouseOptionsManager.Instance.ActivateAddWallsMode();
        GenerateMap();
        GenerateRequest();
    }

    const int NumberOfTimeIntervals = 16;
    
    async void StartLevelTimer()
    {
        var secondsPerInterval = (int)(timeInSeconds * 1000 / NumberOfTimeIntervals);

        var currentInterval = 0;
        timerText.text = GetTimeAsClock(currentInterval);

        while (currentInterval < NumberOfTimeIntervals)
        {
            await Task.Delay(secondsPerInterval);
            currentInterval++;
            timerText.text = GetTimeAsClock(currentInterval);
        }
        
        CompleteGame();
    }

    string GetTimeAsClock(int numberOfElapsedIntervals)
    {
        return numberOfElapsedIntervals switch
        {
            0 => "9:00 AM",
            1 => "9:30 AM",
            2 => "10:00 AM",
            3 => "10:30 AM",
            4 => "11:00 AM",
            5 => "11:30 AM",
            6 => "12:00 PM",
            7 => "12:30 PM",
            8 => "1:00 PM",
            9 => "1:30 PM",
            10 => "2:00 PM",
            11 => "2:30 PM",
            12 => "3:00 PM",
            13 => "3:30 PM",
            14 => "4:00 PM",
            15 => "4:30 PM",
            _ => "5:00 PM"
        };
    }

    void CompleteGame()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            return;
        }
        
        InputManager.Instance.SetInputType(InputType.Null);
        HideGeneralUI();
        GameOverManager.Instance.DisplayPanel();
    }

    void GenerateMap()
    {
        var levelLayouts = Resources.LoadAll<LevelLayoutData>("Level Layouts").ToList();

        var randomIndex = Random.Range(0, levelLayouts.Count);

        var levelLayout = levelLayouts[randomIndex];

        NodeManager.Instance.CreateNodesFromLevelLayout(levelLayout);

        var centrePosition = new Vector2((NodeManager.Instance.Width - 1f) / 2f, (NodeManager.Instance.Height - 1f) / 2f);
        Camera.main.transform.position = new Vector3(centrePosition.x, centrePosition.y, Camera.main.transform.position.z);
    }

    void GenerateRequest()
    {
        RequestManager.Instance.GenerateRequest();
    }

    void ShowGeneralUI()
    {
        timerPanel.SetActive(true);
        scorePanel.SetActive(true);
        submitButton.SetActive(true);
    }
    
    void HideGeneralUI()
    {
        timerPanel.SetActive(false);
        scorePanel.SetActive(false);
        submitButton.SetActive(false);
    }
}
