using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class AlwaysTrueOption : DialogueOption
{
    public AlwaysTrueOption()
    {
        AlwaysTrueCondition alwaysTrueCondition = ScriptableObject.CreateInstance<AlwaysTrueCondition>();
        conditions.Add(alwaysTrueCondition);
    }
}