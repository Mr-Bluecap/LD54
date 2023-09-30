using UnityEngine;
using UnityEngine.UI;

public class RoomCategoryButton : MonoBehaviour
{
    [SerializeField]
    Image buttonImage;
    
    RoomCategory roomCategory;
        
    public void SetupButton(RoomCategory roomCategory)
    {
        this.roomCategory = roomCategory;
        var newMaterial = new Material(roomCategory.RoomColour);
        
        newMaterial.shader = Shader.Find("UI/Default");
        
        buttonImage.material = newMaterial;
    }

    public void OnClicked()
    {
        RoomTypeButtonManager.Instance.SelectCategory(roomCategory);
    }
}