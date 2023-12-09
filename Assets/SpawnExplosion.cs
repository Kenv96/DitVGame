using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosion : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke(nameof(Explode), 3);
    }

    private void Explode()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform);
        }
    }
}
