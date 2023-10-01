using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace.Requests;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RequestManager : MonoBehaviour
{
    public static RequestManager Instance;
    const int MaximumNumberOfRequests = 4;

    Request currentRequest;

    [SerializeField]
    TextMeshProUGUI requirementsTextPrefab;

    [SerializeField]
    Transform requirementsHolder;

    [SerializeField]
    AudioSource correctAudio;
    
    [SerializeField]
    AudioSource wrongAudio;

    Dictionary<RoomCategory, int> minimumNumberOfRoomsPerCategory;
    
    void Awake()
    {
        Instance = this;
    }

    [ContextMenu("Generate")]
    public void GenerateRequest()
    {
        List<RequestCondition> requestConditions;
        
        try
        {
            requestConditions = CreateRequestConditions();
        }
        catch (Exception e)
        {
            requestConditions = CreateSimpleConditionList();
        }

        currentRequest = new Request(requestConditions);
        DisplayRequirements();
    }

    List<RequestCondition> CreateRequestConditions()
    {
        var selectedConditions = new List<RequestCondition>();
        minimumNumberOfRoomsPerCategory = new Dictionary<RoomCategory, int>();

        foreach (var category in RoomTypeAssignmentManager.Instance.RoomCategories)
        {
            minimumNumberOfRoomsPerCategory.Add(category, 0);
        }
        
        var totalRoomSquares = RoomManager.Instance.AllRoomSquares.Count;

        List<RequestCondition> allConditions = Resources.LoadAll<RequestCondition>("Request Conditions").ToList();
        
        var listOfUsedRoomTypes = new List<RoomType>();

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

            if (selectedConditions.Contains(potentialCondition) || (potentialCondition.RoomType != null && listOfUsedRoomTypes.Contains(potentialCondition.RoomType)))
            {
                isRequestComplete = IsRequestComplete(GetNumberOfRequiredSquares(), totalRoomSquares, selectedConditions.Count);
                allConditions.Remove(potentialCondition);
                continue;
            }

            var numberOfRoomSquaresRequiredForCondition = 0;
            
            if (potentialCondition.RoomType != null)
            {
                numberOfRoomSquaresRequiredForCondition = potentialCondition.MinimumNumberOfRoomsRequired();

                if (GetNumberOfRequiredSquares() + numberOfRoomSquaresRequiredForCondition > totalRoomSquares)
                {
                    isRequestComplete = IsRequestComplete(GetNumberOfRequiredSquares(), totalRoomSquares, selectedConditions.Count);
                    allConditions.Remove(potentialCondition);
                    continue;
                }
            }
            else
            {
                var maxNumberOfRoomRequired = potentialCondition.MaximumNumberOfRoomsRequired();

                if (maxNumberOfRoomRequired < minimumNumberOfRoomsPerCategory[potentialCondition.RoomCategory])
                {
                    isRequestComplete = IsRequestComplete(GetNumberOfRequiredSquares(), totalRoomSquares, selectedConditions.Count);
                    allConditions.Remove(potentialCondition);
                    continue;
                }
            }
            
            selectedConditions.Add(potentialCondition);

            if (potentialCondition.RoomType != null)
            {
                listOfUsedRoomTypes.Add(potentialCondition.RoomType);
                minimumNumberOfRoomsPerCategory[GetCategoryForType(potentialCondition.RoomType)] += numberOfRoomSquaresRequiredForCondition;
            }
            else
            {
                var minimumSquaresNeededForCategory = potentialCondition.MinimumNumberOfRoomsRequired();
                minimumNumberOfRoomsPerCategory[potentialCondition.RoomCategory] = Mathf.Min(
                    minimumNumberOfRoomsPerCategory[potentialCondition.RoomCategory], minimumSquaresNeededForCategory);
            }
            
            allConditions.Remove(potentialCondition);
            isRequestComplete = IsRequestComplete(GetNumberOfRequiredSquares(), totalRoomSquares, selectedConditions.Count);
        }

        return selectedConditions;
    }

    List<RequestCondition> CreateSimpleConditionList()
    {
        var allConditions = Resources.LoadAll<RequestCondition>("Request Conditions").ToList();
        var randomIndex = Random.Range(0, allConditions.Count);

        var newConditionsList = new List<RequestCondition> { allConditions[randomIndex] };

        return newConditionsList;
    }
    
    int GetNumberOfRequiredSquares()
    {
        var totalSquares = 0;
        
        foreach (var pair in minimumNumberOfRoomsPerCategory)
        {
            totalSquares += pair.Value;
        }

        return totalSquares;
    }

    RoomCategory GetCategoryForType(RoomType type)
    {
        foreach (var category in RoomTypeAssignmentManager.Instance.RoomCategories)
        {
            foreach (var possibleType in category.RoomTypes)
            {
                if (possibleType == type)
                {
                    return category;
                }
            }
        }

        return RoomTypeAssignmentManager.Instance.RoomCategories[0];
    }

    bool IsRequestComplete(int numberOfRequiredRooms, int totalNumberOfRoomSquares, int numberOfRequests)
    {
        if (numberOfRequiredRooms == 0)
        {
            return false;
        }

        if (numberOfRequests == MaximumNumberOfRequests)
        {
            return true;
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
