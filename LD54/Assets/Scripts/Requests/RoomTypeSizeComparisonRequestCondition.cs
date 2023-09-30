﻿using System.Collections.Generic;
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
}