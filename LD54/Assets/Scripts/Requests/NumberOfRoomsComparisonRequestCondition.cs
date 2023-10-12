
using System.Collections.Generic;

public abstract class NumberOfRoomsComparisonRequestCondition : RequestCondition
{
    public override bool IsConditionMet(List<Room> roomLayout)
    {
        var numberOfRooms = GetTotalNumberOfRooms(roomLayout);

        return comparisonType switch
        {
            ComparisonType.Less_Than => IsLessThan(numberOfRooms),
            ComparisonType.Equal_To => IsEqualTo(numberOfRooms),
            ComparisonType.Greater_Than => IsGreaterThan(numberOfRooms),
            _ => false
        };
    }
    
    public override int MinimumNumberOfRoomsRequired()
    {
        return comparisonType switch
        {
            ComparisonType.Less_Than => 1,
            ComparisonType.Equal_To => size,
            ComparisonType.Greater_Than => size + 1,
            _ => size
        };
    }
        
    public override int MaximumNumberOfRoomsRequired()
    {
        return comparisonType switch
        {
            ComparisonType.Less_Than => size - 1,
            ComparisonType.Equal_To => size,
            ComparisonType.Greater_Than => MaximumNumberOfRequiredRoomsPerCondition,
            _ => size
        };
    }
    
    protected abstract int GetTotalNumberOfRooms(List<Room> roomLayout);
    
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
}