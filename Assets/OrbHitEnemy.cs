using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHitEnemy : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
        }
    }
}
