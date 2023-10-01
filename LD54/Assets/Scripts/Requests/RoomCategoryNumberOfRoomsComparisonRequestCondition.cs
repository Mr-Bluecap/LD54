using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Requests
{
    [CreateAssetMenu(fileName = "New Condition", menuName = "Conditions/Room Category/Room Count Comparison")]
    public class RoomCategoryNumberOfRoomsComparisonRequestCondition : NumberOfRoomsComparisonRequestCondition
    {
        public RoomCategory roomCategory;
        public override RoomType RoomType => null;
        public override RoomCategory RoomCategory => roomCategory;
        
        protected override int GetTotalNumberOfRooms(List<Room> roomLayout)
        {
            var totalNumberOfRooms = 0;

            foreach (var room in roomLayout)
            {
                if (roomCategory.RoomTypes.Contains(room.CurrentRoomType))
                {
                    totalNumberOfRooms++;
                }
            }

            return totalNumberOfRooms;
        }
    }
}