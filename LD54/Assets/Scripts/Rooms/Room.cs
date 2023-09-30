using System.Collections.Generic;

[System.Serializable]
public struct Room
{
    public int Size => RoomSquares.Count;
    public string Name;
    
    public List<RoomSquare> RoomSquares;

    public Room(string name)
    {
        Name = name;
        RoomSquares = new List<RoomSquare>();
    }
    
    public void AddRoomSquare(RoomSquare newRoomSquare)
    {
        RoomSquares.Add(newRoomSquare);
    }
}