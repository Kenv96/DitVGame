using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public void Disable(GameObject obj)
    {
        obj.SetActive(false);
    }
}
