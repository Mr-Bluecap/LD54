using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RequestCondition : ScriptableObject
{
    public abstract bool IsConditionMet(List<Room> roomLayout);
}

