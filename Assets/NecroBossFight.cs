using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NecroBossFight : MonoBehaviour, IEnemy
{
    bool canBeHit, moving, canMove, stunned;
    public string enemyName;
    public float health;
    public float maxHealth;
    public float GCD;
    public int attackDamage;
    public FloatingHealth healthBar;
    public GameObject[] platforms;
    //private bool dead;
    private bool acting;
    public TransformScriptObj[] clonePositions, bossPositions;
    public AudioClip[] sounds;
    AudioSource sound;
    public Transform player, attackPoint, wallPoint;
    public TransformScriptObj cloneGoTo, moveGoTo;
    public Vector3 target;
    public GameObject basicAttack, ball, deathexplosion, deathpart, clonefog, shield, fadeToWhite;
    public bool fighting, attacking, canAttack;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.Find("Armature_0(Clone)").GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fighting && !attacking)
        {
            TargetPlayer();
            FacePlayer();
            if (canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, 3.5f * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                moving = true;
            }
        }
        if (fighting && attacking)
        {
            if(canAttack) Attack();
            FacePlayer();
            moving = false;
        }
        CheckDistance();
        anim.SetBool("Moving", moving);
    }

    public void StartFight()
    {
        canMove = true;
        health = maxHealth;
        canAttack = true;
        fighting = true;
        basicAttack.SetActive(false);
        sound.PlayOneShot(sounds[0]);
        InvokeRepeating(nameof(Platforms), 6f, 25f);
        InvokeRepeating(nameof(Teleport), 7f, 7f);
        InvokeRepeating(nameof(TPInRoom), 11f, 11f);
    }

    void Platforms()
    {
        List<int> selectedIndices = new List<int>();
        List<GameObject> selectedObjects = new List<GameObject>();

        while (selectedIndices.Count < 20)
        {
            int randomIndex = Random.Range(0, platforms.Length);

            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                selectedObjects.Add(platforms[randomIndex]);
            }
        }

        // Now, the selectedObjects list contains 20 unique game objects.
        foreach (var obj in selectedObjects)
        {
            // Do something with the selected objects here.
            obj.GetComponent<MovePlatform>().StartLower();
        }
    }

    void Attack()
    {
        Invoke(nameof(ActivateAttack), 0.4f);
        Invoke(nameof(DeactivateAttack), 1.2f);
        canAttack = false;
        canMove = false;
        Invoke(nameof(ResetAttack), GCD);
        anim.SetTrigger("Attack");
    }
    
    void ActivateAttack()
    {
        basicAttack.SetActive(true);
    }

    void DeactivateAttack()
    {
        basicAttack.SetActive(false);
    }

    void ResetAttack()
    {
        canAttack = true;
        canMove = true;
    }
    public void TakeDamage(int damage)
    {
        if (canBeHit)
        {
            health -= damage;
            canBeHit = false;
            healthBar.UpdateHealthBar(health, maxHealth);
            if (health / maxHealth < .5f)
            {
                StartCoroutine(Die());
            }
        }

        Invoke(nameof(ResetHit), 0.3f);
    }

    private void Teleport()
    {
        if (!acting)
        {
            transform.position = player.transform.position - transform.forward * 3;
        }
    }

    private void TPInRoom()
    {
        acting = true;
        canMove = false;
        moving = false;
        float x = Random.Range(-518, -481);
        float z = Random.Range(-247, -209);
        transform.position = new Vector3(x, 0, z);
        Invoke(nameof(ShootAttack), 0.5f);
        Invoke(nameof(ShootAttack), 1.0f);
        Invoke(nameof(ShootAttack), 1.5f);
        Invoke(nameof(ResetAction), 2.0f);
        Invoke(nameof(Teleport), 2.10f);
    }

    private void ShootAttack()
    {
        //anim.SetTrigger("attack_short_001");
        Vector3 directionOfShot = target - attackPoint.position;
        Vector3 directionOfShot1 = target - attackPoint.position;
        Vector3 directionOfShot2 = target - attackPoint.position;

        GameObject[] currentSpell = new GameObject[] { Instantiate(ball, attackPoint.position, Quaternion.identity), Instantiate(ball, attackPoint.position, Quaternion.identity), Instantiate(ball, attackPoint.position, Quaternion.identity) };

        currentSpell[0].transform.forward = directionOfShot.normalized;
        currentSpell[1].transform.forward = directionOfShot1.normalized;
        currentSpell[2].transform.forward = directionOfShot2.normalized;

        currentSpell[1].transform.Rotate(0,30,0,Space.World);
        currentSpell[2].transform.Rotate(0, -30, 0, Space.World);

        currentSpell[0].GetComponent<Rigidbody>().AddForce(currentSpell[0].transform.forward * 0.2f, ForceMode.Impulse);
        currentSpell[1].GetComponent<Rigidbody>().AddForce(currentSpell[1].transform.forward * 0.2f, ForceMode.Impulse);
        currentSpell[2].GetComponent<Rigidbody>().AddForce(currentSpell[2].transform.forward * 0.2f, ForceMode.Impulse);
    }

    private void ResetAction()
    {
        acting = false;
        canMove = true;
    }

    void TargetPlayer()
    {
        target = new Vector3(player.position.x, player.position.y + 1.25f, player.position.z);
    }

    private void FacePlayer()
    {
        if (player)
        {
            Vector3 faceDir = player.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(faceDir);
            transform.rotation = lookRot;
            //transform.rotation = Quaternion.Euler(0,lookRot.y,0);
        }
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        float thresholdDistance = 2.0f;
        if (distance < thresholdDistance)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
    }

    public void DrainHealth(float damage)
    {
        health -= Time.deltaTime * damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health / maxHealth < .5f)
        {
            StartCoroutine(Die());
        }
    }

    public void ResetHit()
    {
        canBeHit = true;
    }

    private IEnumerator Die()
    {
        //dead = true;
        fighting = false;
        CancelInvoke();
        transform.position = new Vector3(-500, 10, -218.5f);
        //anim.SetTrigger("Die");
        shield.SetActive(true);
        sound.PlayOneShot(sounds[4]);
        QuestManager.GetInstance().EnemyKilled(enemyName);
        MusicManager.GetInstance().Quiet(1);
        yield return new WaitForSeconds(2);
        sound.PlayOneShot(sounds[3]);
        yield return new WaitForSeconds(4);
        fadeToWhite.GetComponent<Animator>().Play("ScreenFadeOUT");
        yield return new WaitForSeconds(1);
        TeleportPlayer();
        Destroy(gameObject, 10.0f);
    }

    private void TeleportPlayer()
    {
        player.gameObject.transform.position = new Vector3(-242,-359,60);
        //GameObject.Find("ExploreCam").GetComponent<CinemachineFreeLook>().m_XAxis = 180f;
    }
}
