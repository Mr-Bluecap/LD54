using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Catalogue", menuName = "Rooms/Room Catalogue")]
public class RoomCatalogue : ScriptableObject
{
    public RoomType defaultRoomType;
    [SerializeField]
    Material defaultRoomColour;
    
    public List<RoomCategory> roomCategories;

    void OnEnable()
    {
        AssignCategoryColourToRoomTypes();
    }
    
    void AssignCategoryColourToRoomTypes()
    {
        defaultRoomType.AssignCategoryColour(defaultRoomColour);
        
        foreach (var category in roomCategories)
        {
            foreach (var roomType in category.RoomTypes)
            {
                roomType.AssignCategoryColour(category.RoomColour);
            }
        }
    }
}
