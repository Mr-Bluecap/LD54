using UnityEngine;

namespace Inputs
{
    public class RemoveWallsInputHandler : InputHandler
    {
        readonly LayerMask roomLineLayerMask;
        
        public RemoveWallsInputHandler(InputManager inputManager, LayerMask roomLineLayerMask) : base(inputManager)
        {
            this.roomLineLayerMask = roomLineLayerMask;
        }

        public override void OnMouseHover()
        {
            
        }

        public override void OnLeftMouseDown()
        {
            
        }

        public override void OnLeftMouseHold()
        {
            var line = GetRoomLineFromMousePosition();

            if (line == null)
            {
                return;
            }
            
            RoomLineManager.Instance.RemoveLine(line);
            AudioManager.Instance.PlayDestroyWall();
            RoomManager.Instance.CreateRooms(true);
        }

        public override void OnLeftMouseUp()
        {

        }
        
        RoomLine GetRoomLineFromMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, roomLineLayerMask))
            {
                return hit.collider.GetComponent<RoomLine>();
            }

            return null;
        }
    }
}