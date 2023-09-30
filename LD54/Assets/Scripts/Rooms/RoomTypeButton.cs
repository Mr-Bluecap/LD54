using DefaultNamespace;
using TMPro;
using UnityEngine;

public class RoomTypeButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;

    RoomType roomType;
    
    public void SetupButton(RoomType roomType)
    {
        nameText.text = roomType.Name;
        this.roomType = roomType;
    }
    
    public void OnClicked()
    {
        //TODO: Move to "tabs"
        MouseOptionsManager.ActivateAssignRoomTypeMode();
        RoomTypeAssignmentManager.Instance.SelectRoomType(roomType);
    }
}