using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomTypeAssignmentManager : MonoBehaviour
{
    public static RoomTypeAssignmentManager Instance;

    [SerializeField]
    RoomTypeButton roomTypeButton;
        
    [SerializeField]
    RoomCategory defaultRoomCategory;

    [SerializeField]
    List<RoomCategory> roomCategories;

    [SerializeField]
    Transform buttonHolder;
    
    public RoomType CurrentSelectedRoomType => currentSelectedRoomType;
    public RoomType DefaultRoomType => defaultRoomCategory.RoomTypes[0];
    
    RoomType currentSelectedRoomType;

    void Awake()
    {
        Instance = this;

        AssignCategoryColourToRoomTypes();
    }

    void Start()
    {
        foreach (var category in roomCategories)
        {
            SpawnButtonsForCategory(category);
        }
    }

    void AssignCategoryColourToRoomTypes()
    {
        defaultRoomCategory.RoomTypes[0].AssignCategoryColour(defaultRoomCategory.RoomColour);
        
        foreach (var category in roomCategories)
        {
            foreach (var roomType in category.RoomTypes)
            {
                roomType.AssignCategoryColour(category.RoomColour);
            }
        }
    }

    void SpawnButtonsForCategory(RoomCategory category)
    {
        foreach (var roomType in category.RoomTypes)
        {
            var button = Instantiate(roomTypeButton, buttonHolder);
            button.SetupButton(roomType);
        }
    }

    public void SelectRoomType(RoomType roomType)
    {
        Debug.LogWarning($"[ROOM TYPES] Selected room type: {roomType.Name}");
        currentSelectedRoomType = roomType;
    }
}

[System.Serializable]
public struct RoomCategory
{
    public string Name;
    public Material RoomColour;
    public List<RoomType> RoomTypes;
}

[System.Serializable]
public class RoomType
{
    public string Name;
    public Sprite Icon;
    public Material RoomColour => roomColour;

    Material roomColour;

    public void AssignCategoryColour(Material newRoomColour)
    {
        roomColour = newRoomColour;
    }
}