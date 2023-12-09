using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBossAddMove : MonoBehaviour, IEnemy
{
    public BoxCollider hitbox;

    public float damage;
    bool canBeHit;
    public string enemyName;
    public float health;
    public float maxHealth;
    public float speed;
    public Vector3 direction;
    public bool touchingPlayer;
    
    FloatingHealth healthBar;
    public AudioClip[] sounds;
    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 18.0f);
        healthBar = GetComponentInChildren<FloatingHealth>();   
        canBeHit = true;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (touchingPlayer) PlayerMove.GetInstance().DrainHealth(damage);
    }

    public void TakeDamage(int damage)
    {
        if (canBeHit)
        {
            health -= damage;
            canBeHit = false;
            healthBar.UpdateHealthBar(health, maxHealth);
        }

        Invoke(nameof(ResetHit), 0.3f);


        if (health < 0)
        {
            Die();
        }
    }

    public void DrainHealth(float damage)
    {
        health -= Time.deltaTime * damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health < 0)
        {
            Die();
        }
    }

    public void ResetHit()
    {
        canBeHit = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) touchingPlayer = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) touchingPlayer = false;
    }

}
