using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transform", menuName = "General/Transform")]
public class TransformScriptObj : ScriptableObject
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
