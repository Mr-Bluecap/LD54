using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RequestGenerator : MonoBehaviour
{
    public static RequestGenerator Instance;

    public Request currentRequest;

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
}
