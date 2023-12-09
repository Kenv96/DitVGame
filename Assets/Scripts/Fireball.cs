using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject explosion;

    private void Start()
    {
        Destroy(gameObject,0.3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
