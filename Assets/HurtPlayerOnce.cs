using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerOnce : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) PlayerMove.GetInstance().TakeDamage(damage);
    }
}
