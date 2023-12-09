using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCloudBG : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3((transform.position.x - 1f), transform.position.y, transform.position.z);
        if(transform.position.x <= -2000)
        {
            transform.position = new Vector3(1000, transform.position.y, transform.position.z);
        }
    }
}
