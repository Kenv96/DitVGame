using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeBash : MonoBehaviour
{
    public PlayerMove player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            if(player.canEnrage)
            {
                player.canEnrage = false;
                player.enraged = true;
                player.rage = 10;
                player.ResetEnrage();
            }
        }
    }
}
