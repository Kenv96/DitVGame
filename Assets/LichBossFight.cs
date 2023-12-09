using UnityEngine;

public class LichBossFight : MonoBehaviour, IEnemy
{
    bool canBeHit, moving, canMove, stunned;
    public string enemyName;
    public float health;
    public float maxHealth;
    FloatingHealth healthBar;
    public GameObject[] clones;
    private bool dead;
    public TransformScriptObj[] clonePositions, bossPositions;
    public AudioClip[] sounds;
    AudioSource sound;
    public Transform player, attackPoint, wallPoint;
    public TransformScriptObj cloneGoTo, moveGoTo;
    public Vector3 target, savedPos; 
    public GameObject basicAttack, wallAttack, deathexplosion, deathpart, clonefog, currclonefog, blockade, postConvo;
    public bool canWallAttack, canCloneAttack, inCloneAttack;
    private Animator anim;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        health = maxHealth;
        anim = GetComponent<Animator>();
        canWallAttack = true; canCloneAttack = true; canBeHit = true; moving = false;
        healthBar = GetComponentInChildren<FloatingHealth>();
        healthBar.UpdateHealthBar(health, maxHealth);
        player = GameObject.Find("Player").transform;
        wallPoint = GameObject.Find("WallSpawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            float newX = Mathf.Lerp(transform.position.x, moveGoTo.position.x, Time.deltaTime * 3);
            float newZ = Mathf.Lerp(transform.position.z, moveGoTo.position.z, Time.deltaTime * 3);
            transform.position = new Vector3(newX, 2.8f , newZ);
            if (System.Math.Abs(moveGoTo.position.x - transform.position.x) < 0.1 && System.Math.Abs(moveGoTo.position.z - transform.position.z) < 0.1)
            {
                StopMove();
            }
        }

        if (stunned)
        {
            anim.SetBool("damage_001", true);
        }
        else
        {
            anim.SetBool("damage_001", false);
        }
        FacePlayer();
    }

    public void StartFight()
    {
        player = PlayerMove.GetInstance().gameObject.transform;
        target = new Vector3(player.position.x, player.position.y, player.position.z);
        InvokeRepeating(nameof(TakeTurn), 1.0f, 2.0f);
        InvokeRepeating(nameof(TargetPlayer), 0.0f, 1.0f);
    }
    void TargetPlayer()
    {
        target = new Vector3(player.position.x, player.position.y + 1.25f, player.position.z);
    }

    void TakeTurn()
    {
        if(!inCloneAttack && !moving && !stunned && !dead)
        {
            Debug.Log("Boss attacks");
            if (canWallAttack && canCloneAttack)
            {
                int attack = Random.Range(1, 101);
                if (attack > 75)
                {
                    SummonClones();
                }
                else if (attack >= 50 && attack < 75)
                {
                    SummonWall();
                }
                else if (attack < 50 && attack >= 15)
                {
                    ShootAttack();
                }
                else
                {
                    Move();
                }
            }
            else if (!canWallAttack && canCloneAttack)
            {
                int attack = Random.Range(1, 101);
                if (attack >= 66)
                {
                    SummonClones();
                }
                else if (attack >= 33)
                {
                    ShootAttack();
                }
                else
                {
                    Move();
                }
            }
            else if (canWallAttack && !canCloneAttack)
            {
                int attack = Random.Range(1, 101);
                if (attack >= 66)
                {
                    SummonWall();
                }
                else if(attack >= 33)
                {
                    ShootAttack();
                }
                else
                {
                    Move();
                }
            }
            else
            {
                int attack = Random.Range(1, 101);
                if (attack >= 50)
                {
                    ShootAttack();
                }
                else
                {
                    Move();
                }
            }
        }
    }

    private void ShootAttack()
    {
        anim.SetTrigger("attack_short_001");
        Vector3 directionOfShot = target - attackPoint.position;

        GameObject currentSpell = Instantiate(basicAttack, attackPoint.position, Quaternion.identity);
        currentSpell.transform.forward = directionOfShot.normalized;

        currentSpell.GetComponent<Rigidbody>().AddForce(directionOfShot.normalized * 0.2f, ForceMode.Impulse);
    }

    private void SummonWall()
    {
        anim.SetTrigger("attack_other");
        Instantiate(wallAttack, wallPoint.position, Quaternion.identity);
        canWallAttack = false;
        Invoke(nameof(ResetWallCD),21.0f);
    }

    void ResetWallCD()
    {
        canWallAttack = true;
    }

    private void SummonClones()
    {
        int clonePos = Random.Range(0, 4);
        switch (clonePos)
        {
            case 0:
                Instantiate(clones[0], clonePositions[0].position, Quaternion.Euler(clonePositions[0].rotation));
                Instantiate(clones[1], clonePositions[1].position, Quaternion.Euler(clonePositions[1].rotation));
                Instantiate(clones[2], clonePositions[2].position, Quaternion.Euler(clonePositions[2].rotation));
                Instantiate(clones[3], clonePositions[3].position, Quaternion.Euler(clonePositions[3].rotation));
                break;
            case 1:
                Instantiate(clones[0], clonePositions[1].position, Quaternion.Euler(clonePositions[1].rotation));
                Instantiate(clones[1], clonePositions[2].position, Quaternion.Euler(clonePositions[2].rotation));
                Instantiate(clones[2], clonePositions[3].position, Quaternion.Euler(clonePositions[3].rotation));
                Instantiate(clones[3], clonePositions[0].position, Quaternion.Euler(clonePositions[0].rotation));
                break;
            case 2:
                Instantiate(clones[0], clonePositions[2].position, Quaternion.Euler(clonePositions[2].rotation));
                Instantiate(clones[1], clonePositions[3].position, Quaternion.Euler(clonePositions[3].rotation));
                Instantiate(clones[2], clonePositions[0].position, Quaternion.Euler(clonePositions[0].rotation));
                Instantiate(clones[3], clonePositions[1].position, Quaternion.Euler(clonePositions[1].rotation));
                break;
            case 3:
                Instantiate(clones[0], clonePositions[3].position, Quaternion.Euler(clonePositions[3].rotation));
                Instantiate(clones[1], clonePositions[0].position, Quaternion.Euler(clonePositions[0].rotation));
                Instantiate(clones[2], clonePositions[1].position, Quaternion.Euler(clonePositions[1].rotation));
                Instantiate(clones[3], clonePositions[2].position, Quaternion.Euler(clonePositions[2].rotation));
                break;
            default:
                break;
        }
        sound.PlayOneShot(sounds[1]);
        currclonefog = Instantiate(clonefog, transform.position + new Vector3(0, 1.2f), transform.rotation);
        inCloneAttack = true;
        savedPos = new Vector3(transform.position.x,transform.position.y, transform.position.z);
        transform.position = cloneGoTo.position;
        canCloneAttack= false;
    }

    public void ExitClone()
    {
        Destroy(currclonefog);
        inCloneAttack = false;
        stunned = true;
        Invoke(nameof(ResetCloneCD), 15.0f);
        Invoke(nameof(BreakStun), 4.0f);
        transform.position = savedPos;
    }
    
    private void ResetCloneCD()
    {
        canCloneAttack = true;
    }

    private void BreakStun()
    {
        stunned = false;
    }

    private void Move()
    {
        sound.PlayOneShot(sounds[0]);
        int move = Random.Range(0, 4);
        moveGoTo = bossPositions[move];
        moving = true;
    }

    private void StopMove()
    {
        moving = false;
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

    private void FacePlayer()
    {
        if (player)
        {
            Vector3 faceDir = player.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(faceDir);
            transform.rotation = lookRot;
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
        dead = true;
        anim.SetBool("dead", true);
        deathpart.SetActive(true);
        QuestManager.GetInstance().EnemyKilled(enemyName);
        //MusicManager.GetInstance().Quiet(1);
        Invoke(nameof(StartDia), 2.9f);
        Destroy(gameObject, 3.0f);
    }

    void StartDia()
    {
        blockade.SetActive(false);
        postConvo.SetActive(true);
    }

    void OnDestroy()
    {
        Instantiate(deathexplosion, transform.position + new Vector3(0,1.2f), transform.rotation);
    }
}
