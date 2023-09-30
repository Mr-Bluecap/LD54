using System.Collections.Generic;
using UnityEngine;

public class RoomSquare : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    RoomTypeInfoHolder infoHolder;
    
    public string Name { get; private set; }
    public List<RoomLine> OpenRoomLines { get; private set; }

    public void Setup(Node originNode, List<RoomLine> openRoomLines)
    {
        Name = originNode.name;
        Vector3 newPosition = originNode.XY + Vector2.one * 0.5f;
        newPosition.z = 1;
        transform.localPosition = newPosition;
        OpenRoomLines = openRoomLines;
    }

    public void SetMaterial(Material newMaterial)
    {
        sprite.material = newMaterial;
    }

    public void DisplayRoomDetails(Room room)
    {
        infoHolder.DisplayInformation(room.CurrentRoomType);
    }
}