using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
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

    void Awake()
    {
        Instance = this;
    }
    
    public void DisplayPanel()
    {
        gameOverPanel.SetActive(true);
        scorePanel.SetActive(true);

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
