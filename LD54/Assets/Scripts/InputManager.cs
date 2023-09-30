using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    LayerMask nodeLayerMask;
    
    [SerializeField]
    LayerMask cameraMovementLayerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var node = GetNodeFromMousePosition();
            if (node == null)
            {
                return;
            }
            
            LineDrawer.Instance.StartLine(node);
        }
        else if (Input.GetMouseButton(0))
        {
            var possiblePosition = GetWorldPositionFromMousePosition(Input.mousePosition);
            if (possiblePosition == null)
            {
                return;
            }

            var position = (Vector3)possiblePosition;
            
            LineDrawer.Instance.UpdateLine(position);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var node = GetNodeFromMousePosition();
            if (node == null)
            {
                LineDrawer.Instance.DestroyLine();
                return;
            }

            if (LineDrawer.Instance.FinishLine(node, out var line))
            {
                //Update room list
                RoomLineManager.Instance.AddLine(line);
                RoomCalculator.Instance.CreateRooms();
            }
        }

        
        if (!LineDrawer.Instance.IsDrawing && Input.GetMouseButtonUp(1))
        {
            var line = GetRoomLineFromMousePosition();

            if (line == null)
            {
                return;
            }
            
            RoomLineManager.Instance.RemoveLine(line);
            RoomCalculator.Instance.CreateRooms();
        }
    }
    
    Node GetNodeFromMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, nodeLayerMask))
        {
            return hit.collider.GetComponent<Node>();
        }

        return null;
    }
    
    RoomLine GetRoomLineFromMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, nodeLayerMask))
        {
            return hit.collider.GetComponent<RoomLine>();
        }

        return null;
    }
    
    Vector3? GetWorldPositionFromMousePosition(Vector2 mousePosition)
    {
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, cameraMovementLayerMask))
        {
            return hit.point;
        }

        return null;
    }
}
