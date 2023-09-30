using System;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public static LineDrawer Instance;
    
    public bool IsDrawing { get; private set; }
    
    [SerializeField]
    RoomLine line;

    [SerializeField]
    Transform lineHolder;

    RoomLine currentLine;

    void Awake()
    {
        Instance = this;
    }

    public void StartLine(Node startNode)
    {
        IsDrawing = true;
        currentLine = Instantiate(line, lineHolder);
        currentLine.SetStartNode(startNode);

        var startPosition = startNode.transform.position;
        
        currentLine.SetPosition(0, startPosition);
        UpdateLine(startPosition);
    }

    public void UpdateLine(Vector3 newEndPosition)
    {
        if (currentLine == null)
        {
            return;
        }
        
        currentLine.SetPosition(1, newEndPosition);
    }

    public bool FinishLine(Node endNode, out RoomLine line)
    {
        line = null;
        
        if (currentLine == null)
        {
            return false;
        }
        
        var endPosition = endNode.transform.position;
        
        if (endPosition == currentLine.GetPosition(0))
        {
            DestroyLine();
            return false;
        }
        
        UpdateLine(endPosition);
        currentLine.SetEndNode(endNode);
        currentLine.AddCollisionToLine();
        line = currentLine;
        currentLine = null;
        IsDrawing = false;
        return true;
    }

    public RoomLine CreateLine(Node startNode, Node endNode)
    {
        StartLine(startNode);
        FinishLine(endNode, out var newLine);
        startNode.AddConnection(endNode);
        endNode.AddConnection(startNode);

        newLine.name = $"Line: [{startNode.XY.x}, {startNode.XY.y}  -  {endNode.XY.x}, {endNode.XY.y}]";
        
        return newLine;
    }

    public RoomLine CreateEmptyLine(Node startNode, Node endNode)
    {
        StartLine(startNode);
        FinishLine(endNode, out var newLine);
        
        newLine.name = $"Empty Line: [{startNode.XY.x}, {startNode.XY.y}  -  {endNode.XY.x}, {endNode.XY.y}]";
        
        newLine.DisableLine();
        return newLine;
    }

    public void DestroyLine()
    {
        DestroyLine(currentLine);
        currentLine = null;
    }

    public void DestroyLine(RoomLine lineToDestroy, bool removeConnections = true)
    {
        IsDrawing = false;
        
        if (lineToDestroy == null)
        {
            return;
        }

        if (removeConnections)
        {
            lineToDestroy.StartNode.RemoveConnection(lineToDestroy.EndNode);
            lineToDestroy.EndNode?.RemoveConnection(lineToDestroy.StartNode);
        }
        
        lineToDestroy.DestroyLine();
    }
}
