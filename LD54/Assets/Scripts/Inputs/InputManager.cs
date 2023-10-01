using System;
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

    [SerializeField]
    Texture2D normalCursor;
    
    [SerializeField]
    Texture2D drawCursor;
    
    [SerializeField]
    Texture2D deleteCursor;
    
    [SerializeField]
    Texture2D paintCursor;

    Dictionary<InputType, InputHandler> allInputHandlers;

    InputHandler currentInputHandler;
    
    void Awake()
    {
        Instance = this;
        
        SetupInputHandlers();
        SetInputType(InputType.AddWalls);
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    void SetupInputHandlers()
    {
        allInputHandlers = new Dictionary<InputType, InputHandler>
        {
            { InputType.AddWalls, new AddWallsInputHandler(this, nodeLayerMask, cameraMovementLayerMask) },
            { InputType.RemoveWalls, new RemoveWallsInputHandler(this, nodeLayerMask) },
            { InputType.AssignRoomType, new AssignRoomTypeInputHandler(this, roomLayerMask) },
            { InputType.Null, null}
        };
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentInputHandler?.OnLeftMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            currentInputHandler?.OnLeftMouseHold();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentInputHandler?.OnLeftMouseUp();
        }
        
        currentInputHandler?.OnMouseHover();
    }

    public void SetInputType(InputType newInputType)
    {
        Debug.LogWarning($"[INPUT] New Input Type: {newInputType.ToString()}");
        currentInputHandler = allInputHandlers[newInputType];

        switch (newInputType)
        {
            case InputType.AddWalls:
                Cursor.SetCursor(drawCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InputType.RemoveWalls:
                Cursor.SetCursor(deleteCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InputType.AssignRoomType:
                Cursor.SetCursor(paintCursor, Vector2.zero, CursorMode.Auto);
                break;
            case InputType.Null:
            default:
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}

public enum InputType
{
    AddWalls,
    RemoveWalls,
    AssignRoomType,
    Null
}
