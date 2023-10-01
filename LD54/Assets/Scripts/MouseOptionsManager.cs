using UnityEngine;

namespace DefaultNamespace
{
    public class MouseOptionsManager : MonoBehaviour
    {
        public static MouseOptionsManager Instance;

        [SerializeField]
        AudioSource clearRoomAudio;
        
        void Awake()
        {
            Instance = this;
        }
        
        public void ActivateAddWallsMode()
        {
            InputManager.Instance.SetInputType(InputType.AddWalls);
        }
        
        public void ActivateRemoveWallsMode()
        {
            InputManager.Instance.SetInputType(InputType.RemoveWalls);
        }
        
        public void ActivateAssignRoomTypeMode()
        {
            InputManager.Instance.SetInputType(InputType.AssignRoomType);
        }
        
        public void ActivateClearAssignRoomTypeMode()
        {
            InputManager.Instance.SetInputType(InputType.AssignRoomType);
            RoomTypeAssignmentManager.Instance.SelectRoomType(RoomTypeAssignmentManager.Instance.DefaultRoomType);
        }

        public void ResetRoom()
        {
            RoomLineManager.Instance.RemoveAllLines();
            ActivateAddWallsMode();
            RoomManager.Instance.CreateRooms(false);
            
            clearRoomAudio.Stop();
            clearRoomAudio.Play();
        }
    }
}