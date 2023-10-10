using System.Collections;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    const int NumberOfTimeIntervals = 16;
    
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

    [SerializeField]
    AudioSource clockTickAudio;
    
    [SerializeField]
    AudioSource whistleAudio;

    void Awake()
    {
        Instance = this;
        
        secondsPerInterval = timeInSeconds / NumberOfTimeIntervals;
        currentInterval = NumberOfTimeIntervals;
    }

    void Start()
    {
        HideGeneralUI();
        ClientManager.Instance.HideClient();
    }

    public void StartGame()
    {
        GameOverManager.Instance.HideDisplay();
        ScoreManager.Instance.ResetScore();
        ShowGeneralUI();
        CreateNewSequence();

        clockTickAudio.Stop();
        whistleAudio.Stop();
        ResetTimer();
    }

    public void CreateNewSequence()
    {
        RoomLineManager.Instance.DestroyAllLines();
        MouseOptionsManager.Instance.ActivateAddWallsMode();
        GenerateMap();
        GenerateRequest();
        ClientManager.Instance.CreateNewClient();
    }

    // IEnumerator StartLevelTimer()
    // {
    //     var currentInterval = 0;
    //     timerText.text = GetTimeAsClock(currentInterval);
    //
    //     while (currentInterval < NumberOfTimeIntervals)
    //     {
    //         yield return new WaitForSeconds(secondsPerInterval);
    //         currentInterval++;
    //         timerText.text = GetTimeAsClock(currentInterval);
    //
    //         if (currentInterval == NumberOfTimeIntervals - 1)
    //         {
    //             clockTickAudio.Play();
    //         }
    //         else if (currentInterval == NumberOfTimeIntervals)
    //         {
    //             clockTickAudio.Stop();
    //             whistleAudio.Play();
    //         }
    //     }
    //     
    //     CompleteGame();
    // }

    float secondsPerInterval;

    float currentTimer = 0;
    int currentInterval = 0;

    void ResetTimer()
    {
        currentTimer = 0;
        currentInterval = 0;
        
        timerText.text = GetTimeAsClock(currentInterval);
    }
    
    void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer < secondsPerInterval || currentInterval >= NumberOfTimeIntervals)
        {
            return;
        }

        currentTimer = 0;
        
        currentInterval++;
        timerText.text = GetTimeAsClock(currentInterval);

        if (currentInterval == NumberOfTimeIntervals - 1)
        {
            clockTickAudio.Play();
        }
        else if (currentInterval == NumberOfTimeIntervals)
        {
            clockTickAudio.Stop();
            whistleAudio.Play();
            
            CompleteGame();
        }
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
        ClientManager.Instance.HideClient();
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
