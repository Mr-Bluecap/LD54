using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomTypeAssignmentManager : MonoBehaviour
{
    public static RoomTypeAssignmentManager Instance;

    [SerializeField]
    RoomCatalogue roomCatalogue;

    public RoomType CurrentSelectedRoomType => currentSelectedRoomType;
    public RoomType DefaultRoomType => roomCatalogue.defaultRoomType;
    public List<RoomCategory> RoomCategories => roomCatalogue.roomCategories;
    
    RoomType currentSelectedRoomType;

    void Awake()
    {
        Instance = this;
    }

    public void SelectRoomType(RoomType roomType)
    {
        Debug.LogWarning($"[ROOM TYPES] Selected room type: {roomType.Name}");
        currentSelectedRoomType = roomType;
    }
}