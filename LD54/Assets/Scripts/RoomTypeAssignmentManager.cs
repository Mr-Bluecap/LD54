using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomTypeAssignmentManager : MonoBehaviour
{
    public static RoomTypeAssignmentManager Instance;

    [SerializeField]
    RoomCatalogue roomCatalogue;
    
    [SerializeField]
    RoomTypeButton roomTypeButton;

    [SerializeField]
    Transform buttonHolder;
    
    public RoomType CurrentSelectedRoomType => currentSelectedRoomType;
    public RoomType DefaultRoomType => roomCatalogue.defaultRoomType;
    
    RoomType currentSelectedRoomType;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (var category in roomCatalogue.roomCategories)
        {
            SpawnButtonsForCategory(category);
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