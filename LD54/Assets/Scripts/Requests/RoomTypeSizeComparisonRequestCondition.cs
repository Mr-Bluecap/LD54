using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Condition", menuName = "Conditions/Room Type/Size Comparison")]
public class RoomTypeSizeComparisonRequestCondition : SizeComparisonRequestCondition
{
    public RoomType roomType;
    
    protected override int GetTotalRoomSize(List<Room> roomLayout)
    {
        var totalSize = 0;

        foreach (var room in roomLayout)
        {
            if (room.CurrentRoomType == roomType)
            {
                totalSize += room.Size;
            }
        }

        return totalSize;
    }

    public override int NumberOfRoomsRequired()
    {
        return comparisonType switch
        {
            ComparisonType.Less_Than => 1,
            ComparisonType.Equal_To => size,
            ComparisonType.Greater_Than => size + 1,
            _ => size
        };
    }

    public override string ConditionDescription()
    {
        var comparisonString = base.ConditionDescription();
        return $"{roomType.Name} {comparisonString}";
    }
}