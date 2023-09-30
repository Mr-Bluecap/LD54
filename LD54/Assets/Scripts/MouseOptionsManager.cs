using UnityEngine;

namespace DefaultNamespace
{
    public class MouseOptionsManager : MonoBehaviour
    {
        public void ActivateAddWallsMode()
        {
            InputManager.Instance.SetInputType(InputType.AddWalls);
        }
        
        public void ActivateRemoveWallsMode()
        {
            InputManager.Instance.SetInputType(InputType.RemoveWalls);
        }
        
        public static void ActivateAssignRoomTypeMode()
        {
            InputManager.Instance.SetInputType(InputType.AssignRoomType);
        }
    }
}