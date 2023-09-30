using System.Collections.Generic;
using Inputs;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    [SerializeField]
    LayerMask nodeLayerMask;
    
    [SerializeField]
    LayerMask cameraMovementLayerMask;
    
    [SerializeField]
    LayerMask roomLayerMask;

    Dictionary<InputType, InputHandler> allInputHandlers;

    InputHandler currentInputHandler;
    
    void Awake()
    {
        Instance = this;
        
        SetupInputHandlers();
        SetInputType(InputType.AddWalls);
    }

    void SetupInputHandlers()
    {
        allInputHandlers = new Dictionary<InputType, InputHandler>
        {
            { InputType.AddWalls, new AddWallsInputHandler(this, nodeLayerMask, cameraMovementLayerMask) },
            { InputType.RemoveWalls, new RemoveWallsInputHandler(this, nodeLayerMask) },
            { InputType.AssignRoomType, new AssignRoomTypeInputHandler(this, roomLayerMask) }
        };
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentInputHandler.OnLeftMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            currentInputHandler.OnLeftMouseHold();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentInputHandler.OnLeftMouseUp();
        }
        
        currentInputHandler.OnMouseHover();
    }

    public void SetInputType(InputType newInputType)
    {
        Debug.LogWarning($"[INPUT] New Input Type: {newInputType.ToString()}");
        currentInputHandler = allInputHandlers[newInputType];
    }
}

public enum InputType
{
    AddWalls,
    RemoveWalls,
    AssignRoomType
}
