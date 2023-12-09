using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarHeal : MonoBehaviour
{
    public float heal;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMove.GetInstance().HealOverTime(heal);
        }
    }
}
