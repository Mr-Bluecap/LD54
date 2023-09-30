using System.Collections.Generic;

public abstract class SizeComparisonRequestCondition : RequestCondition
{
    public ComparisonType comparisonType;
    public int size;

    public override bool IsConditionMet(List<Room> roomLayout)
    {
        var roomSize = GetTotalRoomSize(roomLayout);

        return comparisonType switch
        {
            ComparisonType.Less_Than => IsLessThan(roomSize),
            ComparisonType.Equal_To => IsEqualTo(roomSize),
            ComparisonType.Greater_Than => IsGreaterThan(roomSize),
            _ => false
        };
    }

    protected abstract int GetTotalRoomSize(List<Room> roomLayout);

    bool IsLessThan(int roomSize)
    {
        return roomSize < size;
    }

    bool IsEqualTo(int roomSize)
    {
        return roomSize == size;
    }

    bool IsGreaterThan(int roomSize)
    {
        return roomSize > size;
    }
}

public enum ComparisonType
{
    Less_Than,
    Equal_To,
    Greater_Than
}