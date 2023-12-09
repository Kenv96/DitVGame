using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject toFollow;
    // Update is called once per frame
    void Update()
    {
        transform.position = toFollow.transform.position;   
    }
}
