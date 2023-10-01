using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace.Requests
{
    [CreateAssetMenu(fileName = "New Condition", menuName = "Conditions/Room Category/Size Comparison")]
    public class RoomCategorySizeComparisonRequestCondition : SizeComparisonRequestCondition
    {
        public RoomCategory roomCategory;
        public override RoomType RoomType => null;
        public override RoomCategory RoomCategory => roomCategory;

        protected override int GetTotalSize(List<Room> roomLayout)
        {
            var totalSize = 0;

            foreach (var room in roomLayout)
            {
                if (roomCategory.RoomTypes.Contains(room.CurrentRoomType))
                {
                    totalSize += room.Size;
                }
            }

            return totalSize;
        }
        
        public override string ConditionDescription()
        {
            var comparisonString = base.ConditionDescription();
            var colour = $"#{roomCategory.RoomColour.color.ToHexString()}";
            return $"Total <color={colour}>{roomCategory.Name}</color> Tiles {comparisonString}";
        }
    }
}