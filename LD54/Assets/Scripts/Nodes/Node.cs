using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    NodeRenderer nodeRenderer;
    
    
    NodeInputState inputState;
    HashSet<Node> connectedNodes;
    
    public Vector2Int XY { get; private set; }

    void Awake()
    {
        connectedNodes = new HashSet<Node>();
    }

    public void SetPosition(int x, int y)
    {
        XY = new Vector2Int(x, y);
    }
    
    public void AddConnection(Node nodeToAdd)
    {
        connectedNodes.Add(nodeToAdd);
        SetIsConnected(connectedNodes.Count > 0);
    }

    public void RemoveConnection(Node nodeToRemove)
    {
        connectedNodes.Remove(nodeToRemove);
        SetIsConnected(connectedNodes.Count > 0);
    }

    public bool HasConnection(Node node)
    {
        return connectedNodes.Contains(node);
    }

    void SetIsConnected(bool isConnected)
    {
        SetInputStateFlag(NodeInputState.Connected, isConnected);
    }
    
    public void SetIsSelected(bool isSelected)
    {
        SetInputStateFlag(NodeInputState.Selected, isSelected);
    }
    
    public void SetIsHovered(bool isHovered)
    {
        SetInputStateFlag(NodeInputState.Hovered, isHovered);
    }
    
    void SetInputStateFlag(NodeInputState state, bool isFlagOn)
    {
        if (isFlagOn)
        {
            inputState |= state; //Tries to add the flag
        }
        else
        {
            inputState &= ~state; //Tries to remove the flag
        }
            
        nodeRenderer.UpdateRenderer(inputState);
    }
}