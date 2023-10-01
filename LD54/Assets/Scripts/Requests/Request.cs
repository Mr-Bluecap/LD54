using System.Collections.Generic;

[System.Serializable]
public class Request
{
    public List<RequestCondition> RequestConditions;

    public Request(List<RequestCondition> requestConditions)
    {
        RequestConditions = requestConditions;
    }

    public float GetCompletedRequestsAsPercentage(List<Room> roomLayout)
    {
        var numberOfRequestsComplete = 0;
        
        foreach (var condition in RequestConditions)
        {
            var isConditionComplete = condition.IsConditionMet(roomLayout);

            if (isConditionComplete)
            {
                numberOfRequestsComplete++;
            }
        }

        return (float)numberOfRequestsComplete / RequestConditions.Count;
    }
}