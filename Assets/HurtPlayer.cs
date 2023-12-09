using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public float damage;
    public bool dot;
    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) PlayerMove.GetInstance().DrainHealth(damage);
    }
}
