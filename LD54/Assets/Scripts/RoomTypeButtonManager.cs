using UnityEngine;
using UnityEngine.UI;

public class RoomTypeButtonManager : MonoBehaviour
{
    public static RoomTypeButtonManager Instance;
    
    [SerializeField]
    RoomCategoryButton roomCategoryButton;

    [SerializeField]
    Transform categoryButtonHolder;
    
    [SerializeField]
    RoomTypeButton roomTypeButton;

    [SerializeField]
    CustomLayoutGroup roomTypeLayoutGroup;

    [SerializeField]
    Image roomTypeBoard;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        SpawnRoomCategoryButtons();
    }

    void SpawnRoomCategoryButtons()
    {
        var hasSelectedCategory = false;
        foreach (var category in RoomTypeAssignmentManager.Instance.RoomCategories)
        {
            var button = Instantiate(roomCategoryButton, categoryButtonHolder);
            button.SetupButton(category);

            if (!hasSelectedCategory)
            {
                hasSelectedCategory = true;
                button.OnClicked();
            }
        }
    }
    
    void SpawnButtonsForCategory(RoomCategory category)
    {
        roomTypeLayoutGroup.ClearList();
        
        foreach (var roomType in category.RoomTypes)
        {
            var button = Instantiate(roomTypeButton);
            roomTypeLayoutGroup.AddElementToList(button.GetComponent<RectTransform>());
            button.SetupButton(roomType);
        }

        var newMaterial = new Material(category.RoomColour);
        newMaterial.shader = Shader.Find("UI/Default");
        roomTypeBoard.material = newMaterial;
    }

    public void SelectCategory(RoomCategory category)
    {
        SpawnButtonsForCategory(category);
    }
}
