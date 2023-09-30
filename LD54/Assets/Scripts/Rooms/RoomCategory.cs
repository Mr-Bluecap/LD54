using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Category", menuName = "Rooms/Room Category")]
[System.Serializable]
public class RoomCategory : ScriptableObject
{
    public string Name;
    public Material RoomColour;
    public List<RoomType> RoomTypes;
}