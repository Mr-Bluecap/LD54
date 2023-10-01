using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace.Requests
{
    [CreateAssetMenu(fileName = "New Condition", menuName = "Conditions/Room Type/Room Count Comparison")]
    public class RoomTypeNumberOfRoomsComparisonRequestCondition : NumberOfRoomsComparisonRequestCondition
    {
        public RoomType roomType;
        public override RoomCategory RoomCategory => null;
        public override RoomType RoomType => roomType;
        
        protected override int GetTotalNumberOfRooms(List<Room> roomLayout)
        {
            var totalNumberOfRooms = 0;

            foreach (var room in roomLayout)
            {
                if (room.CurrentRoomType == roomType)
                {
                    totalNumberOfRooms++;
                }
            }

            return totalNumberOfRooms;
        }
        
        public override string ConditionDescription()
        {
            var comparisonString = base.ConditionDescription();
            var colour = $"#{roomType.RoomColour.color.ToHexString()}";
            return $"Number Of <color={colour}>{roomType.Name}s</color> {comparisonString}";
        }
    }
}