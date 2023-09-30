using UnityEngine;

namespace Inputs
{
    public class AddWallsInputHandler : InputHandler
    {
        Node currentSelectedNode;
        Node currentHoveredNode;

        readonly LayerMask nodeLayerMask;
        readonly LayerMask cameraMovementLayerMask;
        
        public AddWallsInputHandler(InputManager inputManager, LayerMask nodeLayerMask, LayerMask cameraMovementLayerMask) : base(inputManager)
        {
            this.nodeLayerMask = nodeLayerMask;
            this.cameraMovementLayerMask = cameraMovementLayerMask;
        }

        public override void OnMouseHover()
        {
            if (!LineDrawer.Instance.IsDrawing)
            {
                return;
            }
            
            var node = GetNodeFromMousePosition();

            if (node == currentHoveredNode)
            {
                return;
            }

            currentHoveredNode?.SetIsHovered(false);
            currentHoveredNode = node;
            
            if (!isNodeOnDiagonal(node))
            {
                currentHoveredNode?.SetIsHovered(true);
            }
        }

        bool isNodeOnDiagonal(Node node)
        {
            if (node == null || currentSelectedNode == null)
            {
                return false;
            }
            
            return node.XY.x != currentSelectedNode.XY.x && node.XY.y != currentSelectedNode.XY.y;
        }

        public override void OnLeftMouseDown()
        {
            var node = GetNodeFromMousePosition();
            if (node == null)
            {
                return;
            }

            currentSelectedNode = node;
            currentSelectedNode.SetIsSelected(true);
            
            LineDrawer.Instance.StartLine(currentSelectedNode);
        }

        public override void OnLeftMouseHold()
        {
            var possiblePosition = GetWorldPositionFromMousePosition(Input.mousePosition);
            if (possiblePosition == null)
            {
                return;
            }

            var position = (Vector3)possiblePosition;
            
            LineDrawer.Instance.UpdateLine(position);
        }

        public override void OnLeftMouseUp()
        {
            currentSelectedNode?.SetIsSelected(false);
            currentHoveredNode?.SetIsHovered(false);

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
                RoomManager.Instance.CreateRooms();
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
}