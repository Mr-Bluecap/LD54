using System.Collections.Generic;

[System.Serializable]
public class Request
{
    public List<RequestCondition> RequestConditions;

    public Request(List<RequestCondition> requestConditions)
    {
        RequestConditions = requestConditions;
    }

    bool EvaluateRequest(List<Room> roomLayout)
    {
        foreach (var condition in RequestConditions)
        {
            var isConditionComplete = condition.IsConditionMet(roomLayout);

            if (!isConditionComplete)
            {
                return false;
            }
        }

        return true;
    }
}