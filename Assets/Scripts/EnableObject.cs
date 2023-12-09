using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour
{
    public void Enable(GameObject obj)
    {
        obj.SetActive(true);
    }
}
