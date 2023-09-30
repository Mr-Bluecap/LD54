using UnityEngine;

[CreateAssetMenu(fileName = "New Room Type", menuName = "Rooms/Room Type")]
[System.Serializable]
public class RoomType : ScriptableObject
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