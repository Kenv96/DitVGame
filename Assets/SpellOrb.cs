using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellOrb : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject hitbox;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitbox.SetActive(false);
        InvokeRepeating(nameof(Shoot), 0.5f, 0.75f);
        Destroy(gameObject, 6.7f);
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.velocity / 1.02f;
    }

    void Shoot()
    {
        hitbox.SetActive(true);
    }
}
