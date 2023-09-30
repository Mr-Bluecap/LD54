using UnityEngine;

public class RequestGenerator : MonoBehaviour
{
    [SerializeField]
    RequestCondition TESTrequest;

    [ContextMenu("Evaluate")]
    void EvaluateRequest()
    {
        var completed = TESTrequest.IsConditionMet(RoomManager.Instance.AllRooms);
        Debug.LogError($"[CONDITION] {completed}");
    }
}
