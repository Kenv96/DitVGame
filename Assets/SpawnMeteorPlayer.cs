using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteorPlayer : MonoBehaviour
{
    public GameObject meteor;
    void Start()
    {
        float x = GameObject.Find("Player").transform.position.x;
        float z = GameObject.Find("Player").transform.position.z;
        transform.position = new Vector3(x, -359, z);
        Invoke(nameof(Spawn), 2.5f);
    }

    void Spawn()
    {
        Instantiate(meteor, new Vector3(transform.position.x, transform.position.y + 17, transform.position.z), Quaternion.identity);
    }
}
