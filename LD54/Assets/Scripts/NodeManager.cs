using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance;
    
    [SerializeField]
    int width;

    [SerializeField]
    int height;
    
    [SerializeField]
    Node node;

    [SerializeField]
    Transform nodeHolder;

    List<Node> allNodes;
    
    Dictionary<int, List<Node>> nodesByRow;
    Dictionary<int, List<Node>> nodesByColumn;

    public int Width => width;
    public int Height => height;

    public List<Node> AllNodes => allNodes;

    void Awake()
    {
        Instance = this;
        allNodes = new List<Node>();
        SetUpDictionaries();
    }
    
    void Start()
    {
        CreateNodes();
    }

    void SetUpDictionaries()
    {
        nodesByRow = new Dictionary<int, List<Node>>();
        nodesByColumn = new Dictionary<int, List<Node>>();
        
        for (int y = 0; y < height; y++)
        {
            nodesByRow.Add(y, new List<Node>());
        }
        
        for (int x = 0; x < width; x++)
        {
            nodesByColumn.Add(x, new List<Node>());
        }
    }

    void CreateNodes()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var newNode = Instantiate(node, nodeHolder);
                newNode.transform.localPosition = new Vector3(x, y);
                
                newNode.SetPosition(x, y);
                newNode.name = $"[{x}, {y}]";
                
                nodesByRow[y].Add(newNode);
                nodesByColumn[x].Add(newNode);
                allNodes.Add(newNode);
            }
        }
    }

    public IEnumerable<Node> GetNodesByColumn(int columnNumber)
    {
        return nodesByColumn[columnNumber];
    }
    
    public IEnumerable<Node> GetNodesByRow(int rowNumber)
    {
        return nodesByRow[rowNumber];
    }

    public Node GetNode(int x, int y)
    {
        var nodesToCheck = GetNodesByColumn(x);

        foreach (var nextNode in nodesToCheck)
        {
            if (nextNode.XY.y == y)
            {
                return nextNode;
            }
        }

        return null;
    }
}
