using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public PlayerMove player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (player.enraged)
            {
                other.gameObject.GetComponent<IEnemy>().TakeDamage(player.meleeDamage * 2);
                player.rage = 0;
                player.enraged= false;
                player.UpdateBar(player.rageBar, player.rage, player.rageTime);
            }
            else
            {
                other.gameObject.GetComponent<IEnemy>().TakeDamage(player.meleeDamage);
            }
            player.GetComponent<AudioSource>().PlayOneShot(player.sounds[6]);
            gameObject.SetActive(false);
        }
    }
}
