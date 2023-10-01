using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Condition", menuName = "Conditions/Room Type/Size Comparison")]
public class RoomTypeSizeComparisonRequestCondition : SizeComparisonRequestCondition
{
    public RoomType roomType;
    public override RoomCategory RoomCategory => null;
    public override RoomType RoomType => roomType;
    
    protected override int GetTotalSize(List<Room> roomLayout)
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

    public override string ConditionDescription()
    {
        var comparisonString = base.ConditionDescription();
        var colour = $"#{roomType.RoomColour.color.ToHexString()}";
        return $"Total <color={colour}>{roomType.Name}</color> Tiles {comparisonString}";
    }
}