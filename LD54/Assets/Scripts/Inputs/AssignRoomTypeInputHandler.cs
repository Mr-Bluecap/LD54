using UnityEngine;

namespace Inputs
{
    public class AssignRoomTypeInputHandler : InputHandler
    {
        readonly LayerMask roomLayerMask;

        Room currentHoveredRoom;
        
        public AssignRoomTypeInputHandler(InputManager inputManager, LayerMask roomLayerMask) : base(inputManager)
        {
            this.roomLayerMask = roomLayerMask;
        }

        public override void OnMouseHover()
        {
            var possibleRoom = GetRoomFromMousePosition();

            if (possibleRoom == currentHoveredRoom)
            {
                return;
            }
            
            currentHoveredRoom?.RemovePotentialRoomType();
            currentHoveredRoom = possibleRoom;
            currentHoveredRoom?.ShowPotentialRoomType(RoomTypeAssignmentManager.Instance.CurrentSelectedRoomType);
        }

        public override void OnLeftMouseDown()
        {
            
        }

        public override void OnLeftMouseHold()
        {
            
        }

        public override void OnLeftMouseUp()
        {
            var possibleRoom = GetRoomFromMousePosition();
            if (possibleRoom == null)
            {
                return;
            }

            var room = possibleRoom;
            room.SetRoomType(RoomTypeAssignmentManager.Instance.CurrentSelectedRoomType);
            AudioManager.Instance.PlayPaint();
        }
        
        Room GetRoomFromMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, roomLayerMask))
            {
                var roomSquare = hit.collider.GetComponent<RoomSquare>();
                return RoomManager.Instance.GetRoomFromRoomSquare(roomSquare);
            }

            return null;
        }
    }
}