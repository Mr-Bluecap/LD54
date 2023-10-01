using System.Collections.Generic;
using UnityEngine;

public abstract class RequestCondition : ScriptableObject
{
    public ComparisonType comparisonType;
    public int size;
    
    public abstract RoomType RoomType { get; }
    public abstract RoomCategory RoomCategory { get; }
    
    protected const int MaximumNumberOfRequiredRoomsPerCondition = 8;

    public abstract bool IsConditionMet(List<Room> roomLayout);

    public abstract int MinimumNumberOfRoomsRequired();
    
    public abstract int MaximumNumberOfRoomsRequired();

    public abstract string ConditionDescription();

    protected int GetRandomNumberOfRooms()
    {
        return Random.Range(0, MaximumNumberOfRequiredRoomsPerCondition);
    }
}

