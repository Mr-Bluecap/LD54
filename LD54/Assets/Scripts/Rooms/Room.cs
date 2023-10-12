using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int Size => RoomSquares.Count;
    public string Name;
    
    public HashSet<RoomSquare> RoomSquares;

    RoomType currentRoomType;
    RoomType potentialRoomType;

    HashSet<string> roomSquareNames;

    public RoomType CurrentRoomType
    {
        get
        {
            if (potentialRoomType != null)
            {
                return potentialRoomType;
            }

            return currentRoomType;
        }
    }
    
    public Room(string name)
    {
        Name = name;
        RoomSquares = new HashSet<RoomSquare>();
        roomSquareNames = new HashSet<string>();
        currentRoomType = RoomTypeAssignmentManager.Instance.DefaultRoomType;
        potentialRoomType = null;
    }
    
    public void AddRoomSquare(RoomSquare newRoomSquare)
    {
        RoomSquares.Add(newRoomSquare);
        roomSquareNames.Add(newRoomSquare.Name);
    }

    public void SetRoomType(RoomType newRoomType)
    {
        currentRoomType = newRoomType;
        SetColoursForRoomSquares(newRoomType.RoomColour);
        DisplayRoomInformation();
        RequestManager.Instance.UpdateRequirements();
    }

    public void ShowPotentialRoomType(RoomType newPotentialRoomType)
    {
        potentialRoomType = newPotentialRoomType;
        SetColoursForRoomSquares(potentialRoomType.RoomColour);
        DisplayRoomInformation();
    }

    public void RemovePotentialRoomType()
    {
        potentialRoomType = null;
        SetColoursForRoomSquares(currentRoomType.RoomColour);
        DisplayRoomInformation();
    }

    void SetColoursForRoomSquares(Material newMaterial)
    {
        foreach (var roomSquare in RoomSquares)
        {
            roomSquare.SetMaterial(newMaterial);
        }
    }

    public bool IsMatchingRoom(Room potentialRoom)
    {
        return roomSquareNames.SetEquals(potentialRoom.roomSquareNames);
    }

    void DisplayRoomInformation()
    {
        GetCentralRoomSquare().DisplayRoomDetails(this);
    }

    public RoomSquare GetCentralRoomSquare()
    {
        var averageX = 0f;
        var averageY = 0f;
        
        foreach (var squareRoom in RoomSquares)
        {
            averageX += squareRoom.transform.position.x;
            averageY += squareRoom.transform.position.y;
        }

        averageX /= RoomSquares.Count;
        averageY /= RoomSquares.Count;
        var averageCentre = new Vector2(averageX, averageY);

        RoomSquare closestRoomSquare = RoomSquares.First();
        var minimumDistance = float.MaxValue;

        foreach (var roomSquare in RoomSquares)
        {
            var distance = Vector2.Distance(roomSquare.transform.position, averageCentre);

            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                closestRoomSquare = roomSquare;
            }
        }

        return closestRoomSquare;
    }
}