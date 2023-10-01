using System.Collections.Generic;
using UnityEngine;

public class RoomLineManager : MonoBehaviour
{
    public static RoomLineManager Instance;

    List<RoomLine> allRoomLines;
    List<RoomLine> allOpenRoomLines;
    
    void Awake()
    {
        Instance = this;

        allRoomLines = new List<RoomLine>();
        allOpenRoomLines = new List<RoomLine>();
    }

    public void AddLine(RoomLine line)
    {
        var isColumn = line.StartNode.XY.x == line.EndNode.XY.x;
        var isRow = line.StartNode.XY.y == line.EndNode.XY.y;

        if (!isRow && !isColumn)
        {
            Debug.LogError("Trying to create diagonal line");
            LineDrawer.Instance.DestroyLine(line, false);
            return;
        }
        
        var nodesToCheck = isColumn
            ? NodeManager.Instance.GetNodesByColumn(line.StartNode.XY.x)
            : NodeManager.Instance.GetNodesByRow(line.StartNode.XY.y);

        Node previousNode = null;
        var hasStartedAddingLines = false;

        Node finalNodeToCheck = null;

        foreach (var currentNode in nodesToCheck)
        {
            if (!hasStartedAddingLines)
            {
                if (currentNode == line.StartNode || currentNode == line.EndNode)
                {
                    hasStartedAddingLines = true;
                    finalNodeToCheck = currentNode == line.StartNode ? line.EndNode : line.StartNode;
                    previousNode = currentNode;
                }
                
                continue;
            }
            
            if (previousNode != null)
            {
                if (currentNode.HasConnection(previousNode))
                {
                    continue;
                }

                if (HasOpenLine(previousNode, currentNode, out var existingOpenLine))
                {
                    allOpenRoomLines.Remove(existingOpenLine);
                    LineDrawer.Instance.DestroyLine(existingOpenLine);
                }
                
                var newLine = LineDrawer.Instance.CreateLine(previousNode, currentNode);
                allRoomLines.Add(newLine);
            }

            if (currentNode == finalNodeToCheck)
            {
                break;
            }

            previousNode = currentNode;
        }

        LineDrawer.Instance.DestroyLine(line, false);
    }

    public void RemoveLine(RoomLine line)
    {
        allRoomLines.Remove(line);
        LineDrawer.Instance.DestroyLine(line);
    }

    public bool HasLine(Node startNode, Node endNode, out RoomLine roomLine)
    {
        foreach (var line in allRoomLines)
        {
            if ((line.StartNode == startNode || line.StartNode == endNode) &&
                (line.EndNode == startNode || line.EndNode == endNode))
            {
                roomLine = line;
                return true;
            }
        }

        roomLine = null;
        return false;
    }
    
    public bool HasOpenLine(Node startNode, Node endNode, out RoomLine roomLine)
    {
        foreach (var line in allOpenRoomLines)
        {
            if ((line.StartNode == startNode || line.StartNode == endNode) &&
                (line.EndNode == startNode || line.EndNode == endNode))
            {
                roomLine = line;
                return true;
            }
        }
        
        roomLine = null;
        return false;
    }

    public RoomLine CreateOpenLine(Node startNode, Node endNode)
    {
        var newLine = LineDrawer.Instance.CreateEmptyLine(startNode, endNode);
        allOpenRoomLines.Add(newLine);
        Debug.LogWarning($"Added Line: {newLine.name}");
        return newLine;
    }

    public void DestroyAllLines()
    {
        for (int i = allRoomLines.Count - 1; i >= 0; i--)
        {
            Destroy(allRoomLines[i].gameObject);
        }
        
        for (int i = allOpenRoomLines.Count - 1; i >= 0; i--)
        {
            Destroy(allOpenRoomLines[i].gameObject);
        }
        
        allRoomLines.Clear();
        allOpenRoomLines.Clear();
    }
    
    public void RemoveAllLines()
    {
        for (int i = allRoomLines.Count - 1; i >= 0; i--)
        {
            LineDrawer.Instance.DestroyLine(allRoomLines[i], true);
        }
        
        for (int i = allOpenRoomLines.Count - 1; i >= 0; i--)
        {
            LineDrawer.Instance.DestroyLine(allOpenRoomLines[i], true);
        }
        
        allRoomLines.Clear();
        allOpenRoomLines.Clear();
    }
}