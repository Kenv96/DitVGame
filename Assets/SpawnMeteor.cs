using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteor : MonoBehaviour
{
    public GameObject meteor;
    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(-253,-230);
        float z = Random.Range(46,66);
        transform.position = new Vector3(x,-359,z);
        Invoke(nameof(Spawn), 2.5f);
    }

    // Update is called once per frame
    void Spawn()
    {
        Instantiate(meteor, new Vector3(transform.position.x, transform.position.y + 17, transform.position.z), Quaternion.identity);
    }
}
