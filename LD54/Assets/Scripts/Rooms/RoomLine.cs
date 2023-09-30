using UnityEngine;

public class RoomLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer line;

    Node startNode;
    Node endNode;

    BoxCollider boxCollider;

    bool isIndestructible;
    
    public Node StartNode => startNode;
    public Node EndNode => endNode;

    public bool IsIndestructible => isIndestructible;

    public void SetStartNode(Node newNode)
    {
        startNode = newNode;
    }
    
    public void SetEndNode(Node newNode)
    {
        endNode = newNode;
    }

    public void SetIsIndestructible(bool isIndestructible)
    {
        this.isIndestructible = isIndestructible;
    }
    
    public void SetPosition(int index, Vector3 position)
    {
        line.SetPosition(index, position);
    }

    public Vector3 GetPosition(int index)
    {
        return line.GetPosition(index);
    }

    public void AddCollisionToLine()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
    }

    public void DisableLine()
    {
        boxCollider.enabled = false;
        line.enabled = false;
    }

    public void DestroyLine()
    {
        Destroy(gameObject);
    }
}
