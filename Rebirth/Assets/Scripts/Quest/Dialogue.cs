using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [TextArea(3, 10)]
    public string[] lines; // 대화의 각 라인
}