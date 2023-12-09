using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    public Vector3 explosionSize;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,2.0f);
        Invoke(nameof(StopDamage), 0.1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
        }
    }

    private void StopDamage()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }
}
