using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RequestManager : MonoBehaviour
{
    public static RequestManager Instance;

    Request currentRequest;

    [SerializeField]
    TextMeshProUGUI requirementsTextPrefab;

    [SerializeField]
    Transform requirementsHolder;

    [SerializeField]
    AudioSource correctAudio;
    
    [SerializeField]
    AudioSource wrongAudio;
    
    void Awake()
    {
        Instance = this;
    }

    [ContextMenu("Generate")]
    public void GenerateRequest()
    {
        var totalRoomSquares = RoomManager.Instance.AllRoomSquares.Count;
        
        var allConditions = Resources.LoadAll<RequestCondition>("Request Conditions").ToList();

        var numberOfRequiredRoomSquares = 0;
        var selectedConditions = new List<RequestCondition>();

        var isRequestComplete = false;

        while (!isRequestComplete)
        {
            if (allConditions.Count == 0)
            {
                isRequestComplete = true;
                continue;
            }
            
            var randomConditionIndex = Random.Range(0, allConditions.Count);

            var potentialCondition = allConditions[randomConditionIndex];

            if (selectedConditions.Contains(potentialCondition))
            {
                isRequestComplete = IsRequestComplete(numberOfRequiredRoomSquares, totalRoomSquares);
                allConditions.Remove(potentialCondition);
                continue;
            }

            var numberOfRoomSquaresRequiredForCondition = potentialCondition.NumberOfRoomsRequired();

            if (numberOfRequiredRoomSquares + numberOfRoomSquaresRequiredForCondition > totalRoomSquares)
            {
                isRequestComplete = IsRequestComplete(numberOfRequiredRoomSquares, totalRoomSquares);
                allConditions.Remove(potentialCondition);
                continue;
            }
            
            //Check room types / categories

            numberOfRequiredRoomSquares += numberOfRoomSquaresRequiredForCondition;
            selectedConditions.Add(potentialCondition);
            allConditions.Remove(potentialCondition);
            isRequestComplete = IsRequestComplete(numberOfRequiredRoomSquares, totalRoomSquares);
        }

        currentRequest = new Request(selectedConditions);
        DisplayRequirements();
    }

    bool IsRequestComplete(int numberOfRequiredRooms, int totalNumberOfRoomSquares)
    {
        if (numberOfRequiredRooms == 0)
        {
            return false;
        }

        var percentageOfRoomsUsed = (float)numberOfRequiredRooms / totalNumberOfRoomSquares;

        var randomIndex = Random.Range(0f, 1f);

        return randomIndex < percentageOfRoomsUsed;
    }

    public void CompleteRequest()
    {
        var requestCompletionPercentage = currentRequest.GetCompletedRequestsAsPercentage(RoomManager.Instance.AllRooms);

        PlayAudio(requestCompletionPercentage);
        
        ScoreManager.Instance.UpdateScore(requestCompletionPercentage);
        GameManager.Instance.CreateNewSequence();
    }

    void PlayAudio(float completionPercentage)
    {
        correctAudio.Stop();
        wrongAudio.Stop();
        
        if (completionPercentage >= 0.5f)
        {
            correctAudio.Play();
        }
        else
        {
            wrongAudio.Play();
        }
    }

    void DisplayRequirements()
    {
        for (int i = requirementsHolder.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(requirementsHolder.GetChild(i).gameObject);
        }
        
        var requirements = GenerateRequestRequirements();

        for (int i = 0; i < requirements.Count; i++)
        {
            var conditionText = Instantiate(requirementsTextPrefab, requirementsHolder);
            conditionText.text = requirements[i];
        }
    }
    
    List<string> GenerateRequestRequirements()
    {
        var requirements = new List<string>();

        foreach (var condition in currentRequest.RequestConditions)
        {
            requirements.Add(condition.ConditionDescription());
        }

        return requirements;
    }
}
