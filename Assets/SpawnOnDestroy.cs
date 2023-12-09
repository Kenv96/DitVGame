using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    public GameObject toSpawn;
    private void OnDestroy()
    {
        Instantiate(toSpawn, transform.position, transform.rotation);
    }
}
