using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : ScriptableObject
{
    public string NPCName;

    public bool[] requirements;

    [TextArea(3, 10)]
    public string[] sentences;

    public int[] speakers;
}
