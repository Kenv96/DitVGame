using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMonster : MonoBehaviour, IEnemy
{
    bool canBeHit, moving, canMove, stunned;
    public string enemyName;
    public float health;
    public float maxHealth;
    public float GCD;
    FloatingHealth healthBar;
    public GameObject[] skeletons, deathpart;
    public GameObject credits, playerobj;
    private bool dead;
    private bool acting;
    public TransformScriptObj[] clonePositions, bossPositions;
    public AudioClip[] sounds;
    AudioSource sound;
    public Transform player, attackPoint, wallPoint;
    public TransformScriptObj cloneGoTo, moveGoTo;
    public Vector3 skeleton1Spawn, skeleton2Spawn;
    public GameObject basicAttack, wallAttack, meteorAttack, meteorAttackP, skeleton, skeleton2, deathexplosion, clonefog, fadeToWhite;
    public bool canWallAttack, canMeteorAttack, canSkeletonAttack;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealth>();
        healthBar.UpdateHealthBar(health, maxHealth);
        canWallAttack = true;
        canMeteorAttack = true;
        canSkeletonAttack = true;
    }

    public void StartFight()
    {
        
        sound.PlayOneShot(sounds[0]);
        InvokeRepeating(nameof(TakeTurn), 8.0f, 6.0f);
        GCD = 7.0f;
        RenderSettings.fog = false;
    }

    void TakeTurn()
    {
        if (!acting)
        {
            int choose = Random.Range(0, 100);
            if (canWallAttack && canMeteorAttack && canSkeletonAttack)
            {
                if (choose < 40)
                {
                    SummonWall();
                }
                if (choose >= 35 && choose <= 65)
                {
                    Meteors();
                }
                if (choose > 60)
                {
                    Skeletons();
                }
                return;
            }
            else if (canWallAttack && canMeteorAttack)
            {
                if(choose < 55)
                {
                    SummonWall();
                }
                if(choose > 45)
                {
                    Meteors();
                }
            }
            else if (canWallAttack && canSkeletonAttack)
            {
                if (choose < 55)
                {
                    SummonWall();
                }
                if (choose > 45)
                {
                    Skeletons();
                }
            }
            else if (canSkeletonAttack && canMeteorAttack)
            {
                if (choose < 55)
                {
                    Meteors();
                }
                if (choose > 45)
                {
                    Skeletons();
                }
            }
            else if(canWallAttack)
            {
                SummonWall();
            }
            else if (canMeteorAttack)
            {
                Meteors();
            }
            else if (canSkeletonAttack)
            {
                Skeletons();
            }
        }
        if(health/maxHealth < .7f)
        {
            GCD = 5.5f;
            if (health / maxHealth < .3f)
            {
                GCD = 4f;
            }
        }
    }

    private void SummonWall()
    {
        acting = true;
        anim.SetTrigger("WallAttack");
        GameObject wall = Instantiate(wallAttack, wallPoint.position + new Vector3(0, 15.5f, 0), Quaternion.identity);
        canWallAttack = false;
        Invoke(nameof(ResetWallCD), 15.0f);
        Invoke(nameof(CanAct), GCD);
    }

    void ResetWallCD()
    {
        canWallAttack = true;
    }

    private void Meteors()
    {
        sound.PlayOneShot(sounds[1]);
        acting = true;
        anim.SetTrigger("MeteorAttack");
        Instantiate(meteorAttack);
        Instantiate(meteorAttack);
        Instantiate(meteorAttack);
        Instantiate(meteorAttack);
        Instantiate(meteorAttackP);
        Invoke(nameof(ResetMeteorCD), 15.0f);
        Invoke(nameof(CanAct), GCD);
    }

    void ResetMeteorCD()
    {
        canMeteorAttack = true;
    }

    private void Skeletons()
    {
        sound.PlayOneShot(sounds[2]);
        acting = true;
        canSkeletonAttack = false;
        Debug.Log("Spawning skel");
        anim.SetTrigger("SkeletonAttack");
        Instantiate(skeleton, skeleton1Spawn, Quaternion.identity);
        Instantiate(skeleton2, skeleton2Spawn, Quaternion.identity);
        Invoke(nameof(ResetSkeletonCD), 30.0f);
        Invoke(nameof(CanAct), GCD);
    }

    void ResetSkeletonCD()
    {
        canSkeletonAttack = true;
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

        if (health < 0 && !dead)
        {
            dead= true;
            StartCoroutine(Die());
        }
    }

    public void CanAct()
    {
        acting = false;
    }
    
    public void Aggro()
    {
        return;
    }

    public void DrainHealth(float damage)
    {
        health -= Time.deltaTime * damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health < 0 && !dead)
        {
            dead = true;
            StartCoroutine(Die());
        }
    }

    public void ResetHit()
    {
        canBeHit = true;
    }

    public IEnumerator Die()
    {
        CancelInvoke();
        sound.PlayOneShot(sounds[3]);
        deathpart[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        MusicManager.GetInstance().ChangeSong(sounds[5]);
        deathpart[1].SetActive(true);
        deathpart[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        sound.PlayOneShot(sounds[4]);
        deathpart[3].SetActive(true);
        yield return new WaitForSeconds(3f);
        fadeToWhite.GetComponent<Animator>().Play("ScreenFadeLasting");
        yield return new WaitForSeconds(2.5f);
        MovePlayer();
        yield return new WaitForSeconds(1f);
        PlayerMove.GetInstance().SwapLilline();
        PlayerMove.GetInstance().ardUnlocked = false;
        PlayerMove.GetInstance().felUnlocked = false;
        Destroy(gameObject, 30);
    }

    public void MovePlayer()
    {
        playerobj.gameObject.transform.position = new Vector3(-225f, -464.5f, -500f);
    }
}
