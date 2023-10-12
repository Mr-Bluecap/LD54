using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomCategoryButton : MonoBehaviour
{
    [SerializeField]
    Image buttonImage;

    [SerializeField]
    TextMeshProUGUI letterText;
    
    RoomCategory roomCategory;
        
    public void SetupButton(RoomCategory roomCategory)
    {
        this.roomCategory = roomCategory;
        var newMaterial = new Material(roomCategory.RoomColour);
        
        newMaterial.shader = Shader.Find("UI/Default");
        
        buttonImage.material = newMaterial;

        var hexColour = newMaterial.color.ToHexString();

        letterText.text = roomCategory.Name.Substring(0, 1);
        //letterText.text = $"<color={hexColour}>{roomCategory.Name.Substring(0, 1)}</color>";
    }

    public void OnClicked()
    {
        RoomTypeButtonManager.Instance.SelectCategory(roomCategory);
    }
}