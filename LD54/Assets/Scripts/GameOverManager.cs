using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameOverManager : MonoBehaviour
{
    [Serializable]
    struct BossComment
    {
        public int scoreThreshold;
        public string[] scoreBossComments;
    }
    
    public static GameOverManager Instance;
    
    const string HighScoreKey = "HighScore";
    
    [SerializeField]
    GameObject gameOverPanel;

    [SerializeField]
    GameObject scorePanel;

    [SerializeField]
    TextMeshProUGUI completedRequestsValueText;
    
    [SerializeField]
    TextMeshProUGUI completedRequestsScoreText;
    
    [SerializeField]
    TextMeshProUGUI partialRequestsValueText;
    
    [SerializeField]
    TextMeshProUGUI partialRequestsScoreText;
    
    [SerializeField]
    TextMeshProUGUI failedRequestsValueText;
    
    [SerializeField]
    TextMeshProUGUI failedRequestsScoreText;
    
    [SerializeField]
    TextMeshProUGUI scoreText;
    
    [SerializeField]
    TextMeshProUGUI highScoreText;

    [SerializeField]
    TextMeshProUGUI tutorialText;

    [SerializeField]
    TextMeshProUGUI bossCommentText;
    
    [SerializeField]
    List<BossComment> bossComments;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void DisplayPanel()
    {
        gameOverPanel.SetActive(true);
        scorePanel.SetActive(true);

        tutorialText.enabled = false;

        completedRequestsValueText.text = $"x {ScoreManager.Instance.CompletedRequestsCount}";
        partialRequestsValueText.text = $"x {ScoreManager.Instance.PartialRequestsCount}";
        failedRequestsValueText.text = $"x {ScoreManager.Instance.FailedRequestsCount}";

        completedRequestsScoreText.text = ScoreManager.Instance.CompletedRequestsScore.ToString();
        partialRequestsScoreText.text = ScoreManager.Instance.PartialRequestsScore.ToString();
        failedRequestsScoreText.text = ScoreManager.Instance.FailedRequestsScore.ToString();

        scoreText.text = CleanedScore(ScoreManager.Instance.TotalScore);

        var currentHighScore = PlayerPrefs.GetInt(HighScoreKey, int.MinValue);
        
        if (currentHighScore >= ScoreManager.Instance.TotalScore)
        {
            highScoreText.text = $"HIGHEST PROFIT: {CleanedScore(currentHighScore)}";
        }
        else
        {
            highScoreText.text = "NEW HIGHEST PROFIT!";
            PlayerPrefs.SetInt(HighScoreKey, ScoreManager.Instance.TotalScore);
        }

        bossCommentText.text = GetRandomBossComment(ScoreManager.Instance.TotalScore);
    }

    string GetRandomBossComment(int score)
    {
        for (int i = 0; i < bossComments.Count; i++)
        {
            if (score >= bossComments[i].scoreThreshold || i == bossComments.Count - 1)
            {
                var randomIndex = Random.Range(0, bossComments[i].scoreBossComments.Length);
                return bossComments[i].scoreBossComments[randomIndex];
            }
        }

        return "...You're fired";
    }
    
    string CleanedScore(int score)
    {
        if (score >= 0)
        {
            return $"${score}";
        }
        
        return $"-${Mathf.Abs(score)}";
    }

    public void HideDisplay()
    {
        gameOverPanel.SetActive(false);
    }

    public void Play()
    {
        GameManager.Instance.StartGame();
    }
}
