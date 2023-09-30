using System.Collections.Generic;
using UnityEngine;

public class RoomCalculator : MonoBehaviour
{
    public static RoomCalculator Instance;

    List<RoomSquare> allRoomSquares;

    List<Room> allRooms;

    void Awake()
    {
        Instance = this;
        
        allRooms = new List<Room>();
        allRoomSquares = new List<RoomSquare>();
        roomSquaresToCheck = new Dictionary<RoomSquare, bool>();
    }

    public void CreateRooms()
    {
        allRooms.Clear();
        allRoomSquares.Clear();
        
        CalculateRoomSquares();
        CalculateRooms();

        for (int i = 0; i < allRooms.Count; i++)
        {
            Debug.LogError($"Room {allRooms[i].Name} has a size of {allRooms[i].Size}");
        }
    }

    Dictionary<RoomSquare, bool> roomSquaresToCheck;
    
    void CalculateRooms()
    {
        roomSquaresToCheck.Clear();
        foreach (var roomSquare in allRoomSquares)
        {
            roomSquaresToCheck.Add(roomSquare, true);   
        }

        foreach (var roomSquare in allRoomSquares)
        {
            if (roomSquaresToCheck[roomSquare])
            {
                var room = CreateRoom(roomSquare);
                allRooms.Add(room);
            }
        }
        
    }

    Room CreateRoom(RoomSquare startingRoomSquare)
    {
        var openSet = new List<RoomSquare>();
        openSet.Add(startingRoomSquare);

        var newRoom = new Room($"Room {startingRoomSquare.Name}");
        
        while (openSet.Count > 0)
        {
            var currentRoomSquare = openSet[0];
            openSet.Remove(currentRoomSquare);
            newRoom.AddRoomSquare(currentRoomSquare);
            roomSquaresToCheck[currentRoomSquare] = false;

            foreach (var roomLine in currentRoomSquare.OpenRoomLines)
            {
                var possibleAdjacentRoom = GetAdjacentRoomSquare(currentRoomSquare, roomLine);

                if (possibleAdjacentRoom == null)
                {
                    continue;
                }

                var adjacentRoom = (RoomSquare)possibleAdjacentRoom;
                
                if (!openSet.Contains(adjacentRoom) && roomSquaresToCheck[adjacentRoom])
                {
                    openSet.Add(adjacentRoom);
                }
            }
        }

        return newRoom;
    }

    RoomSquare? GetAdjacentRoomSquare(RoomSquare originalSquare, RoomLine roomLine)
    {
        foreach (var roomSquare in allRoomSquares)
        {
            if (roomSquare.OpenRoomLines.Contains(roomLine) && !roomSquare.Name.Equals(originalSquare.Name))
            {
                return roomSquare;
            }
        }

        return null;
    }

    void CalculateRoomSquares()
    {
        var totalWidth = NodeManager.Instance.Width;
        var totalHeight = NodeManager.Instance.Height;
        
        foreach (var node in NodeManager.Instance.AllNodes)
        {
            var openRoomLines = new List<RoomLine>();
            var closedRoomLines = new List<RoomLine>();
            
            if (node.XY.x < totalWidth - 1 && node.XY.y < totalHeight - 1)
            {
                var bottomLeftNode = node;
                var topLeftNode = NodeManager.Instance.GetNode(node.XY.x, node.XY.y + 1);
                var bottomRightNode = NodeManager.Instance.GetNode(node.XY.x + 1, node.XY.y);
                var topRightNode = NodeManager.Instance.GetNode(node.XY.x + 1, node.XY.y + 1);

                CreateAllLinesForRoomSquare(bottomLeftNode, topLeftNode, bottomRightNode, topRightNode, ref openRoomLines, ref closedRoomLines);

                var newRoomSquare = new RoomSquare() { Name = bottomLeftNode.name, OpenRoomLines = openRoomLines };
                allRoomSquares.Add(newRoomSquare);
            }
        }
    }

    void CreateAllLinesForRoomSquare(Node bottomLeftNode, Node topLeftNode, Node bottomRightNode, Node topRightNode,
        ref List<RoomLine> openRoomLines, ref List<RoomLine> closedRoomLines)
    {
        CreateLineForRoomSquare(bottomLeftNode, topLeftNode, ref openRoomLines, ref closedRoomLines);
        CreateLineForRoomSquare(bottomLeftNode, bottomRightNode, ref openRoomLines, ref closedRoomLines);
        CreateLineForRoomSquare(bottomRightNode, topRightNode, ref openRoomLines, ref closedRoomLines);
        CreateLineForRoomSquare(topLeftNode, topRightNode, ref openRoomLines, ref closedRoomLines);
    }

    void CreateLineForRoomSquare(Node startNode, Node endNode, ref List<RoomLine> openRoomLines,
        ref List<RoomLine> closedRoomLines)
    {
        RoomLine roomLine;
        
        if (RoomLineManager.Instance.HasLine(startNode, endNode, out roomLine))
        {
            closedRoomLines.Add(roomLine);
        }
        else if (RoomLineManager.Instance.HasOpenLine(startNode, endNode, out roomLine))
        {
            openRoomLines.Add(roomLine);
        }
        else
        {
            var newLine = RoomLineManager.Instance.CreateOpenLine(startNode, endNode);
            openRoomLines.Add(newLine);
        }
    }
}
