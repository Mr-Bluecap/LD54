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
        return roomSize < size && roomSize > 0;
    }

    bool IsEqualTo(int roomSize)
    {
        return roomSize == size;
    }

    bool IsGreaterThan(int roomSize)
    {
        return roomSize > size;
    }
    
    public override string ConditionDescription()
    {
        return comparisonType switch
        {
            ComparisonType.Less_Than => $"< {size}",
            ComparisonType.Equal_To => $"= {size}",
            ComparisonType.Greater_Than => $"> {size}",
            _ => "Whatever"
        };
    }
}

public enum ComparisonType
{
    Less_Than,
    Equal_To,
    Greater_Than
}