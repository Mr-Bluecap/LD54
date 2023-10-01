using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [SerializeField]
    TextMeshProUGUI scoreText;
    
    int completedRequestsScore;
    int partialRequestsScore;
    int failedRequestsScore;
    
    int completedRequestsCount;
    int partialRequestsCount;
    int failedRequestsCount;
    
    public int CompletedRequestsScore => completedRequestsScore;
    public int PartialRequestsScore => partialRequestsScore;
    public int FailedRequestsScore => failedRequestsScore;
    
    public int CompletedRequestsCount => completedRequestsCount;
    public int PartialRequestsCount => partialRequestsCount;
    public int FailedRequestsCount => failedRequestsCount;
    
    public int TotalScore => completedRequestsScore + partialRequestsScore + failedRequestsScore;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DisplayScore();
    }

    public void ResetScore()
    {
        completedRequestsScore = 0;
        partialRequestsScore = 0;
        failedRequestsScore = 0;
    
        completedRequestsCount = 0;
        partialRequestsCount = 0;
        failedRequestsCount = 0;
        
        DisplayScore();
    }

    public void UpdateScore(float requestCompletion)
    {
        if (requestCompletion >= 1)
        {
            completedRequestsCount++;
            completedRequestsScore += GetPointsForRequestCompletion(requestCompletion);
        }
        else if (requestCompletion <= 0)
        {
            failedRequestsCount++;
            failedRequestsScore += GetPointsForRequestCompletion(requestCompletion);
        }
        else
        {
            partialRequestsCount++;
            partialRequestsScore += GetPointsForRequestCompletion(requestCompletion);
        }
        
        DisplayScore();
    }

    int GetPointsForRequestCompletion(float requestCompletion)
    {
        if (requestCompletion <= 0)
        {
            return -500;
        }
        
        return (int)(requestCompletion * 1000);
    }

    void DisplayScore()
    {
        scoreText.text = TotalScore.ToString();
    }
}
