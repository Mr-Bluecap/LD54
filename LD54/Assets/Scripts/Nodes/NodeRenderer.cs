using System;
using UnityEngine;

public class NodeRenderer : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    Material defaultMaterial;

    [SerializeField]
    Material hoveredMaterial;

    [SerializeField]
    Material selectedMaterial;

    [SerializeField]
    Material connectedMaterial;

    public void UpdateRenderer(NodeInputState newState)
    {
        sprite.material = defaultMaterial;

        if (newState.HasFlag(NodeInputState.Selected))
        {
            sprite.material = selectedMaterial;
        }
        else if (newState.HasFlag(NodeInputState.Hovered))
        {
            sprite.material = hoveredMaterial;
        }
        else if (newState.HasFlag(NodeInputState.Connected))
        {
            sprite.material = connectedMaterial;
        }
    }
}

[Flags]
public enum NodeInputState
{
    Hovered = 1 << 0,
    Selected = 1 << 1,
    Connected = 1 << 2
}