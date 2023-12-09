using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBossClone : MonoBehaviour, IEnemy
{
    public bool isCorrect;
    FloatingHealth healthBar;
    public string enemyName;
    public float health;
    public float maxHealth;
    bool canBeHit;
    LichBossFight boss;
    public GameObject failBall;
    public Transform attackPoint;

    // Start is called before the first frame update
    void Start()
    {
        canBeHit = true;
        boss = GameObject.Find("LichBossFight").GetComponent<LichBossFight>();
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealth>();
        healthBar.UpdateHealthBar(health, maxHealth);
        Destroy(gameObject, 8.0f);
        Invoke(nameof(Fail), 7.9f);
    }

    void Update()
    {
        if (!boss.inCloneAttack)
        {
            CancelInvoke();
            Destroy(gameObject);
        }
    }

    public void Succeed()
    {
        CancelInvoke(nameof(Fail));
        boss.ExitClone();
    }

    private void Fail()
    {
        if (isCorrect)
        {
            boss.ExitClone();
        }
        ShootAttack();
    }

    private void ShootAttack()
    {
        Vector3 target = GameObject.Find("Player").transform.position;
        Vector3 directionOfShot = target - attackPoint.position;

        GameObject currentSpell = Instantiate(failBall, attackPoint.position, Quaternion.identity);
        currentSpell.transform.forward = directionOfShot.normalized;

        currentSpell.GetComponent<Rigidbody>().AddForce(directionOfShot.normalized * 0.2f, ForceMode.Impulse);
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
        if (isCorrect)
        {
            Succeed();
        }
        CancelInvoke(nameof(Fail));
        Destroy(gameObject);
    }
}
