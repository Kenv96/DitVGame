using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAggro : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Aggro();
        }
    }
}
