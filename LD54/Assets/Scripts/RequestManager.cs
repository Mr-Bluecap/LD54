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
    
    Dictionary<RoomCategory, RequestCondition> currentCategoryConditions;
    Dictionary<RoomCategory, List<RequestCondition>> roomTypeConditionsByCategory;
    const int MaximumNumberOfCategoryRequests = 2;
    
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
            requestConditions = CreateRequest();
        }
        catch (Exception e)
        {
            requestConditions = CreateSimpleConditionList();
        }

        currentRequest = new Request(requestConditions);
        DisplayRequirements();
    }
    
    List<RequestCondition> CreateRequest()
    {
        currentCategoryConditions = new Dictionary<RoomCategory, RequestCondition>();
        roomTypeConditionsByCategory = new Dictionary<RoomCategory, List<RequestCondition>>();

        foreach (var category in RoomTypeAssignmentManager.Instance.RoomCategories)
        {
            roomTypeConditionsByCategory.Add(category, new List<RequestCondition>());
        }

        var totalRoomSquares = RoomManager.Instance.AllRoomSquares.Count;
        
        
        var listOfCategoryConditions = new List<RequestCondition>();
        var listOfRoomTypeConditions = new List<RequestCondition>();
        
        var allConditions = Resources.LoadAll<RequestCondition>("Request Conditions").ToList();

        foreach (var condition in allConditions)
        {
            if (condition.RoomCategory != null)
            {
                listOfCategoryConditions.Add(condition);
            }
            else
            {
                listOfRoomTypeConditions.Add(condition);
            }
        }
        
        DetermineCategoryConditions(listOfCategoryConditions, totalRoomSquares);
        DetermineRoomTypeCategoryConditions(listOfRoomTypeConditions, totalRoomSquares);

        var selectedConditions = new List<RequestCondition>();

        foreach (var condition in currentCategoryConditions)
        {
            selectedConditions.Add(condition.Value);
        }

        foreach (var category in roomTypeConditionsByCategory)
        {
            foreach (var condition in category.Value)
            {
                selectedConditions.Add(condition);
            }
        }

        return selectedConditions;
    }

    void DetermineRoomTypeCategoryConditions(List<RequestCondition> allRoomTypeConditions, int totalNumberOfRoomSquares)
    {
        var shouldAddRoomTypeCondition = ShouldAddRoomTypeCondition(totalNumberOfRoomSquares);

        while (shouldAddRoomTypeCondition)
        {
            var randomIndex = Random.Range(0, allRoomTypeConditions.Count);
            var possibleCondition = allRoomTypeConditions[randomIndex];

            if (GetPotentialRoomSquaresRequirement(possibleCondition) > totalNumberOfRoomSquares)
            {
                allRoomTypeConditions.Remove(possibleCondition);
                shouldAddRoomTypeCondition = ShouldAddRoomTypeCondition(totalNumberOfRoomSquares);
                continue;
            }

            var possibleCategory = GetCategoryForType(possibleCondition.RoomType);
            
            if (currentCategoryConditions.ContainsKey(possibleCategory))
            {
                if (GetPotentialMinimumNumberOfRoomSquaresRequiredForCategory(possibleCondition) >
                    currentCategoryConditions[possibleCategory].MaximumNumberOfRoomsRequired())
                {
                    allRoomTypeConditions.Remove(possibleCondition);
                    shouldAddRoomTypeCondition = ShouldAddRoomTypeCondition(totalNumberOfRoomSquares);
                    continue;
                }
            }
            
            roomTypeConditionsByCategory[possibleCategory].Add(possibleCondition);
            allRoomTypeConditions.Remove(possibleCondition);
            shouldAddRoomTypeCondition = ShouldAddRoomTypeCondition(totalNumberOfRoomSquares);
        }
    }

    int GetRequiredMinimumNUmberOfRoomSquares()
    {
        var totalRoomSquaresRequired = 0;

        foreach (var category in RoomTypeAssignmentManager.Instance.RoomCategories)
        {
            totalRoomSquaresRequired += GetMinimumNumberOfRoomSquaresRequiredForCategory(category);;
        }

        return totalRoomSquaresRequired;
    }
    
    int GetPotentialRoomSquaresRequirement(RequestCondition condition)
    {
        if (condition.RoomType == null)
        {
            return int.MaxValue;
        }

        var newCategory = GetCategoryForType(condition.RoomType);
        var possibleAddition = condition.MinimumNumberOfRoomsRequired();
        
        var totalRoomSquaresRequired = 0;

        foreach (var category in RoomTypeAssignmentManager.Instance.RoomCategories)
        {
            var roomSquaresToAdd = category == newCategory
                ? GetMinimumNumberOfRoomSquaresRequiredForCategory(category, possibleAddition)
                : GetMinimumNumberOfRoomSquaresRequiredForCategory(category);

            totalRoomSquaresRequired += roomSquaresToAdd;
        }

        return totalRoomSquaresRequired;
    }

    int GetMinimumNumberOfRoomSquaresRequiredForCategory(RoomCategory category, int possibleAddition = 0)
    {
        var currentNumberOfRoomSquaresRequired = 0;
        
        foreach (var requestCondition in roomTypeConditionsByCategory[category])
        {
            currentNumberOfRoomSquaresRequired += requestCondition.MinimumNumberOfRoomsRequired();
        }

        currentNumberOfRoomSquaresRequired += possibleAddition;

        var requiredMinimumFromCategory = currentCategoryConditions.ContainsKey(category)
            ? currentCategoryConditions[category].MinimumNumberOfRoomsRequired()
            : 0;

        return Mathf.Max(currentNumberOfRoomSquaresRequired, requiredMinimumFromCategory);
    }
    
    int GetPotentialMinimumNumberOfRoomSquaresRequiredForCategory(RequestCondition condition)
    {
        var currentNumberOfRoomSquaresRequired = 0;
        var category = GetCategoryForType(condition.RoomType);
        
        foreach (var requestCondition in roomTypeConditionsByCategory[category])
        {
            currentNumberOfRoomSquaresRequired += requestCondition.MinimumNumberOfRoomsRequired();
        }

        currentNumberOfRoomSquaresRequired += condition.MinimumNumberOfRoomsRequired();

        var requiredMinimumFromCategory = currentCategoryConditions.ContainsKey(category)
            ? currentCategoryConditions[category].MinimumNumberOfRoomsRequired()
            : 0;

        return Mathf.Max(currentNumberOfRoomSquaresRequired, requiredMinimumFromCategory);
    }
    
    bool ShouldAddRoomTypeCondition(int totalNumberOfRoomSquares)
    {
        var totalNumberOfConditions = 0;
        totalNumberOfConditions += currentCategoryConditions.Count;

        foreach (var categoryPair in roomTypeConditionsByCategory)
        {
            totalNumberOfConditions += categoryPair.Value.Count;
        }

        if (totalNumberOfConditions == 0)
        {
            return true;
        }

        if (totalNumberOfConditions == MaximumNumberOfRequests)
        {
            return false;
        }

        var numberOfRequiredRooms = GetRequiredMinimumNUmberOfRoomSquares();
        
        var percentageOfRoomsUsed = (float)numberOfRequiredRooms / totalNumberOfRoomSquares;

        var randomIndex = Random.Range(0f, 1f);

        return randomIndex < percentageOfRoomsUsed;
    }

    void DetermineCategoryConditions(List<RequestCondition> allCategoryConditions, int totalNumberOfRoomSquares)
    {
        var shouldAddCategoryCondition = ShouldAddCategoryCondition(allCategoryConditions.Count, totalNumberOfRoomSquares);

        while (shouldAddCategoryCondition)
        {
            var randomIndex = Random.Range(0, allCategoryConditions.Count);
            var possibleCondition = allCategoryConditions[randomIndex];

            if (currentCategoryConditions.ContainsKey(possibleCondition.RoomCategory))
            {
                allCategoryConditions.Remove(possibleCondition);
                shouldAddCategoryCondition = ShouldAddCategoryCondition(allCategoryConditions.Count, totalNumberOfRoomSquares);
                continue;
            }

            var potentialNumberOfRequiredRoomSquares = GetMinimumNumberOfSquaresRequiredForCategories() +
                                                       possibleCondition.MinimumNumberOfRoomsRequired();

            if (potentialNumberOfRequiredRoomSquares >= totalNumberOfRoomSquares)
            {
                allCategoryConditions.Remove(possibleCondition);
                shouldAddCategoryCondition = ShouldAddCategoryCondition(allCategoryConditions.Count, totalNumberOfRoomSquares);
                continue;
            }
            
            currentCategoryConditions.Add(possibleCondition.RoomCategory, possibleCondition);
            allCategoryConditions.Remove(possibleCondition);
            shouldAddCategoryCondition = ShouldAddCategoryCondition(allCategoryConditions.Count, totalNumberOfRoomSquares);
        }
    }

    bool ShouldAddCategoryCondition(int numberOfAvailableConditions, int totalNumberOfRoomSquares)
    {
        if (currentCategoryConditions.Count == MaximumNumberOfCategoryRequests || numberOfAvailableConditions == 0)
        {
            return false;
        }
        
        var randomIndex = Random.Range(0, 1f);

        var newCategoryThreshold = GetMinimumNumberOfSquaresRequiredForCategories() / (float)totalNumberOfRoomSquares + 0.1f;

        return randomIndex > newCategoryThreshold;
    }

    int GetMinimumNumberOfSquaresRequiredForCategories()
    {
        var count = 0;
        
        foreach (var categoryCondition in currentCategoryConditions)
        {
            count += categoryCondition.Value.MinimumNumberOfRoomsRequired();
        }

        return count;
    }

    List<RequestCondition> CreateSimpleConditionList()
    {
        var allConditions = Resources.LoadAll<RequestCondition>("Request Conditions").ToList();
        var randomIndex = Random.Range(0, allConditions.Count);

        var newConditionsList = new List<RequestCondition> { allConditions[randomIndex] };

        return newConditionsList;
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
