using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnPlayer : MonoBehaviour
{
    private bool touchingPlayer;
    public float damage;
    // Update is called once per frame
    void Update()
    {
        if(touchingPlayer)
        {
            PlayerMove.GetInstance().DrainHealth(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) touchingPlayer = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) touchingPlayer = false;
    }
}
