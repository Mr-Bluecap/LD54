using System.Collections.Generic;
using UnityEngine;

public class CustomLayoutGroup : MonoBehaviour
{
    [SerializeField]
    List<RectTransform> childTransforms;

    public void AddElementToList(RectTransform newElementTransform)
    {
        var nextFreeCell = GetNextFreeGridCell();

        if (nextFreeCell == null)
        {
            return;
        }
        
        newElementTransform.SetParent(nextFreeCell);
        newElementTransform.localScale = Vector3.one;
        newElementTransform.offsetMin = Vector2.zero;
        newElementTransform.offsetMax = Vector2.zero;
    }

    public void ClearList()
    {
        foreach (var cell in childTransforms)
        {
            ClearCell(cell);
        }
    }

    void ClearCell(RectTransform cell)
    {
        if (cell.childCount == 0)
        {
            return;
        }
        
        DestroyImmediate(cell.GetChild(0).gameObject);
    }

    RectTransform GetNextFreeGridCell()
    {
        foreach (var cell in childTransforms)
        {
            if (cell.childCount == 0)
            {
                return cell;
            }
        }

        return null;
    }
}
