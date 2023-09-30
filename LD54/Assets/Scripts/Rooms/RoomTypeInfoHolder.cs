using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomTypeInfoHolder : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    
    [SerializeField]
    TextMeshProUGUI roomTypeName;
    
    [SerializeField]
    Image icon;

    public void DisplayInformation(RoomType roomType)
    {
        canvas.enabled = true;
        
        roomTypeName.text = roomType.Name;
        
        icon.enabled = roomType.Icon != null;
        icon.sprite = roomType.Icon;
    }
}