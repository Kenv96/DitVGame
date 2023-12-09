using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    public NecroBossFight enemy;

    // Start is called before the first frame update
    void Awake()
    {
        enemy = GetComponentInParent<NecroBossFight>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<PlayerMove>().TakeDamage(enemy.attackDamage);
            gameObject.SetActive(false);
        }
    }
}
