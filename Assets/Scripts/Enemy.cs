using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    [Header("General")]
    public string enemyName;
    public float health;
    public float maxHealth;
    public bool isDead;
    public int attackDamage;

    [Header("Stats")]
    FloatingHealth healthBar;
    public GameObject stats;
    public GameObject attack;

    Animator anim;

    private NavMeshAgent agent;
    Transform player;

    [Header("Battle")]
    bool attacking;
    bool canAttack;
    bool canBeHit;
    bool resetting;
    bool walking;
    bool stunned;
    public float attackRange;

    Vector3 startingPos;
    public bool searching;
    public bool foundPlayer;

    public AudioClip[] sounds;
    AudioSource sound;

    public GameObject deathEffect;

    public GameObject lootBag;
    public LootTable drops;

    // Start is called before the first frame update
    void Awake()
    {
        canAttack = true;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        health = maxHealth;
        isDead = false;
        canBeHit = true;
        healthBar = GetComponentInChildren<FloatingHealth>();
        healthBar.UpdateHealthBar(health, maxHealth);
        startingPos = transform.position;
        sound = GetComponent<AudioSource>();
        stats.SetActive(false);
    }

    private void Update()
    {
        if (searching)
        {
            if (!stunned)
            {
                if (CheckDistance())
                {
                    Walk();
                    FacePlayer();
                }
                else if (!CheckDistance() && !attacking && canAttack)
                {
                    Attack();
                }

                //Deaggro if too far away
                if (Vector3.Distance(transform.position, startingPos) > 50)
                {
                    DeAggro();
                }
            }
        }

        if (resetting)
        {
            FaceTarget();
            if(Vector3.Distance(transform.position,startingPos) < 0.1f)
            {
                resetting = false;
                health = maxHealth;
                canBeHit = true;
            }
        }

        if (isDead)
        {
            Destroy(gameObject);
        }
    }

    public void Aggro()
    {
        if (!resetting)
        {
            searching = true;
            stats.SetActive(searching);
        }
    }

    public void DeAggro()
    {
        canBeHit = false;
        searching = false;
        agent.SetDestination(startingPos);
        resetting = true;
        stats.SetActive(searching);
    }

    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position) > attackRange;
    }

    private void Walk()
    {
        agent.SetDestination(player.transform.position);
        walking = true;
        anim.SetBool("Walking", walking);
    }

    public void GetStunned()
    {
        if (!resetting && !stunned) stunned = true;
    }

    private void Attack()
    {
        walking= false;
        anim.SetBool("Walking", walking);
        agent.SetDestination(transform.position);
        FacePlayer();
        attacking = true;
        canAttack = false;
        anim.SetTrigger("Attack");
        Invoke("AttackCD", 2);
        Invoke("SpawnAttack",0.4f);
    }

    private void AttackCD()
    {
        attacking = false;
        canAttack = true;
        attack.SetActive(false);
    }

    private void SpawnAttack()
    {
        sound.PlayOneShot(sounds[1]);
        attack.SetActive(true);
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

    private bool ReachedTarget()
    {
        return (Vector3.Distance(transform.position, player.position) < 2.5f);
    }

    private void FacePlayer()
    {
        Vector3 faceDir = player.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(faceDir);
        transform.rotation = lookRot;
    }

    private void FaceTarget()
    {
        Vector3 faceDir = startingPos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(faceDir);
        transform.rotation = lookRot;
    }


    void Die()
    {
        if (lootBag != null)
        {
            GameObject lootDrop = Instantiate(lootBag, new Vector3(transform.position.x, 0.0f, transform.position.z), transform.rotation);
            lootDrop.GetComponent<LootInteract>().possibleLoot = drops;
        }

        QuestManager.GetInstance().EnemyKilled(enemyName);

        if(deathEffect) Instantiate(deathEffect, transform.position, transform.rotation);
        isDead = true;
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            sound.PlayOneShot(sounds[0]);
            foundPlayer = true;
            searching = true;
            stats.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            foundPlayer = false;
            searching = true;
            stats.SetActive(false);
        }
    }
}
