using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public bool safe, control;
    public float moveSpeed;
    public bool canSprint;
    public bool sprinting;
    public GameObject sprintCircle;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public Character currentChar;

    [Header("Attacking")]
    
    public GameObject spell, beam;
    public float shootForce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    bool shooting, beaming;
    public bool readyToShoot;
    public bool isChanneling;
    public bool choosingSpell;
    public int chosenSpell;
    private int lastSpell;
    private int scrollInput;
    private GameObject currentOrb;
    private GameObject currentHeal;
    public GameObject attack, bash;
    public GameObject orb, zone;
    public GameObject spellChoiceUILil;
    public GameObject spellChoiceUIArd;
    public GameObject spellChoiceUIFel;
    public Camera tpCam;
    public Transform attackPoint;
    private float beamChargeTimer;

    public bool allowInvoke = true;

    public float walkSpeed;
    public float sprintSpeed;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public int meleeDamage;
    public bool dead;
    public float damageReduction;

    [Header("Resources")]
    //Lilline
    public float manaRegen;
    public float maxMana;
    public float mana;
    public GameObject manaBarObj;
    public Slider manaBar;
    //Arden Resources
    public bool enraged;
    public bool canEnrage = true;
    public float rage;
    public float rageTime;
    public GameObject rageBarObj;
    public Slider rageBar;
    //Felicia Resources
    public float faith;
    public float maxFaith;
    public float faithRegen;
    public GameObject faithBarObj;
    public Slider faithBar;

    public Slider healthBar;
    public GameObject inv;
    public GameObject charStats;
    public GameObject respawnMenu;
    public Vector3 respawnPoint;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode characterKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool onSlope;
    Vector3 offset = new Vector3(0, 1f, 0);

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    public RaycastHit slopeHit;
    public bool exitingSlope;

    public Transform orientation;
    public Transform modelOrientation;

    public bool inConvo;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public LayerMask doNotHit;
    Rigidbody rb;
    public CapsuleCollider cc;
    float newCenter;

    [Header("Sound")]
    AudioSource sound;
    public AudioClip[] sounds;

    [HideInInspector]
    public Animator anim, lilAnim, ardAnim, felAnim;

    [Header("Characters")]
    public GameObject lillineModel;
    public GameObject ardenModel;
    public GameObject feliciaModel;

    public bool ardUnlocked, felUnlocked;

    public enum Character
    {
        Lilline,
        Arden,
        Felicia
    }

    private static PlayerMove instance;

    private void Start()
    {
        if(!ES3.KeyExists("playerTransform")) ES3.Save("playerTransform", this.transform);
        control = true;
        currentChar = Character.Lilline;
        sound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToShoot = true;
        lilAnim = transform.Find("Lilline/Armature_0(Clone)").GetComponent<Animator>();
        ardAnim = transform.Find("Arden/Armature_0(Clone)").GetComponent<Animator>();
        felAnim = transform.Find("Felicia/Armature_0(Clone)").GetComponent<Animator>();
        anim = lilAnim;
        sprintCircle.SetActive(false);
        canSprint = true;
        cc = GetComponentInChildren<CapsuleCollider>();
        readyToJump = true;
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible= false;
        health = maxHealth;
        mana = maxMana;
        moveSpeed = walkSpeed;
        beamChargeTimer = 1f;
        beam.SetActive(false);
        UpdateBar(manaBar, mana, maxMana);
        UpdateBar(healthBar, health, maxHealth);
        UpdateBar(rageBar, rage, rageTime);
        UpdateBar(faithBar, faith, maxFaith);
        spellChoiceUILil.SetActive(false);
        spellChoiceUIArd.SetActive(false);
        spellChoiceUIFel.SetActive(false);
        feliciaModel.SetActive(false);
        ardenModel.SetActive(false);
    }

    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position + offset, Vector3.down, playerHeight * 0.5f + 0.4f, whatIsGround);
        anim.SetBool("Grounded", grounded);
        onSlope = OnSlope();
        if (!dead)
        {
            if(control) MyInput();
            CheckMana();
            RegenMana();
            RegenFaith();
            RageDecay();
            if (safe)
            {
                HealOverTime(1);
            }
        }

        SpeedControl();
        if(currentChar == Character.Lilline) MoveCenter();//only for sprinting

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        if (!inConvo) anim.SetFloat("Speed", moveDirection.magnitude);
        else anim.SetFloat("Speed", 0);

        anim.SetBool("InAir", !grounded);
        anim.SetBool("IsSprinting", sprinting);
        anim.SetBool("IsChanneling", isChanneling);
        if (currentChar == Character.Lilline) anim.SetBool("IsLil", true);
        if (currentChar == Character.Felicia) anim.SetBool("IsLil", false);
    }

    private void FixedUpdate()
    {
        if(!inConvo) MovePlayer();
    }

    public static PlayerMove GetInstance()
    {
        return instance;
    }

    private void MyInput()
    {
        if(!isChanneling && !inConvo)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentChar != Character.Arden)
        {
            choosingSpell = true;
            Time.timeScale = 0.1f;
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            Time.timeScale = 1.0f;
            choosingSpell = false;
        }

        if (choosingSpell)
        {
            scrollInput = (int)Math.Round(Input.GetAxisRaw("Mouse ScrollWheel"));
            chosenSpell = chosenSpell + scrollInput;
            if (chosenSpell < 0) chosenSpell = 0;
            if (chosenSpell > 1) chosenSpell = 1;
            scrollInput = 0;
            if (currentChar == Character.Lilline)
            {
                spellChoiceUILil.SetActive(true);
            }
            else if (currentChar == Character.Arden)
            {
                spellChoiceUIArd.SetActive(true);
            }
            else if (currentChar == Character.Felicia)
            {
                spellChoiceUIFel.SetActive(true);
            }
        }
        else
        {
            if (currentChar == Character.Lilline)
            {
                spellChoiceUILil.SetActive(false);
            }
            else if (currentChar == Character.Arden)
            {
                spellChoiceUIArd.SetActive(false);
            }
            else if (currentChar == Character.Felicia)
            {
                spellChoiceUIFel.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && canSprint && !sprinting)
        {
            SwapLilline();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && canSprint && !sprinting && ardUnlocked)
        {
            SwapArden();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canSprint && !sprinting && felUnlocked)
        {
            SwapFelicia();
        }
        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canSprint)
        {
            sprinting = true;
            moveSpeed = sprintSpeed;
            sprintCircle.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;
            moveSpeed = walkSpeed;
            sprintCircle.SetActive(false);
        }

        shooting = Input.GetKeyDown(KeyCode.Mouse0);
        if(horizontalInput == 0 && verticalInput == 0)
        {
            beaming = Input.GetKey(KeyCode.Mouse0);
        }

        if (readyToShoot && shooting && (ThirdPersonCam.currentStyle == ThirdPersonCam.CameraStyle.Combat) && mana > 2 && chosenSpell == 0 && currentChar == Character.Lilline)
        {
            TakeShot();
        }

        if (readyToShoot && beaming && (ThirdPersonCam.currentStyle == ThirdPersonCam.CameraStyle.Combat) && mana > 0.1f && chosenSpell == 1 && currentChar == Character.Lilline)
        {
            TheBeam();
        }

        if (readyToShoot && shooting && (ThirdPersonCam.currentStyle == ThirdPersonCam.CameraStyle.Combat) && chosenSpell == 0 && currentChar == Character.Arden)
        {
            Swing();
        }

        if (readyToShoot && shooting && (ThirdPersonCam.currentStyle == ThirdPersonCam.CameraStyle.Combat) && faith > 5.1f && chosenSpell == 0 && currentChar == Character.Felicia)
        {
            Orb();
        }

        if (readyToShoot && shooting && (ThirdPersonCam.currentStyle == ThirdPersonCam.CameraStyle.Combat) && faith > 4.1f && chosenSpell == 1 && currentChar == Character.Felicia)
        {
            Heal();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(isChanneling)
            {
                isChanneling = false;
                beam.SetActive(false);
                beamChargeTimer = 1f;
            }
        }

        if (Input.GetKeyDown(interactKey) && (ThirdPersonCam.currentStyle == ThirdPersonCam.CameraStyle.Basic) && grounded)
        {
            CheckInteractRange();
        }

        if(Input.GetKeyDown(inventoryKey))
        {
            ShowHideInv();
        }

        if (Input.GetKeyDown(characterKey))
        {
            ShowHideChar();
        }
    }

    //#############################################################
    // MOVEMENT
    //#############################################################
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void MoveCenter()
    {
        if (sprinting)
        {
            newCenter = Mathf.Lerp(cc.center.y, 0, Time.deltaTime);//raise during sprint effect
        }

        if (!sprinting)
        {
            newCenter = Mathf.Lerp(cc.center.y, 1, Time.deltaTime * 5);//lower faster than raise
        }

        cc.center = new Vector3(0f, newCenter, 0f);
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope= true;
        anim.SetTrigger("Jump");
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope= false;
    }

    public void AnimSetting(int currentCam)
    {
        if (currentCam == 0)
            anim.SetBool("InCombat", false);
        if(currentCam == 1)
            anim.SetBool("InCombat", true);

    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position + offset, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    //#############################################################
    // END MOVEMENT
    //#############################################################

    //#############################################################
    // INTERACTION
    //#############################################################

    public bool CheckInteractRange()
    {
        RaycastHit hit;
        if(Physics.SphereCast(modelOrientation.position, 1f, modelOrientation.forward, out hit, 2f))
        {
            if (hit.transform.CompareTag("Interact"))
            {
                hit.transform.gameObject.GetComponent<Interactable>().Interact();
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void ShowHideInv()
    {
        if (inv.activeSelf)
        {
            AudioSource.PlayClipAtPoint(sounds[3], transform.position);
            inv.SetActive(false);
        }
        else
        {
            AudioSource.PlayClipAtPoint(sounds[2],transform.position);
            inv.SetActive(true);
        }
    }

    public void ShowHideChar()
    {
        if (charStats.activeSelf)
        {
            charStats.SetActive(false);
        }
        else       
        {
            charStats.SetActive(true);
        }
    }

    //#############################################################
    // END INTERACTION
    //#############################################################

    //#############################################################
    // LILLINE ATTACKS
    //#############################################################

    private void TakeShot()
    {
        readyToShoot = false;
        mana = mana - 2;
        UpdateBar(manaBar, mana, maxMana);

        Ray ray = tpCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 75, doNotHit, QueryTriggerInteraction.Ignore))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionOfShot = targetPoint - attackPoint.position;

        GameObject currentSpell = Instantiate(spell, attackPoint.position, Quaternion.identity);
        currentSpell.transform.forward = directionOfShot.normalized;

        currentSpell.GetComponent<Rigidbody>().AddForce(directionOfShot.normalized * shootForce, ForceMode.Impulse);

        anim.SetTrigger("IsAttacking");

        sound.PlayOneShot(sounds[0], 0.5f);
        sound.PlayOneShot(sounds[1], 1f);
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void TheBeam()
    {
        isChanneling = true;

        if (beamChargeTimer > 0.001f)
        {
            beamChargeTimer -= Time.deltaTime * 2;
        }
        else
        {
            beam.SetActive(true);
            mana -= Time.deltaTime * 1.5f;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
        attack.SetActive(false);
    }

    //#############################################################
    // ARDEN ATTACKS
    //#############################################################
    private void Swing()
    {
        readyToShoot = false;

        anim.SetTrigger("IsAttacking");
        sound.PlayOneShot(sounds[5], 1f);
        sound.PlayOneShot(sounds[4], 1f);
        if (allowInvoke)
        {
            Invoke("SpawnAttack", 0.3f);
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    public void Block()
    {
        bash.SetActive(true);
        damageReduction = .75f;
    }

    public void Unblock()
    {
        bash.SetActive(false);
        damageReduction = 0;
    }

    private void SpawnAttack()
    {
        attack.SetActive(true);
    }

    private void SpawnBash()
    {
        bash.SetActive(true);
    }

    //#############################################################
    // FELICIA ATTACKS
    //#############################################################

    private void Orb()
    {
        //should only ever be one orb
        if (!currentOrb)
        {
            readyToShoot = false;
            faith -= 5;
            UpdateBar(faithBar, faith, maxFaith);

            Ray ray = tpCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, 75, doNotHit, QueryTriggerInteraction.Ignore))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75);

            Vector3 directionOfShot = targetPoint - attackPoint.position;

            GameObject currentSpell = Instantiate(orb, attackPoint.position, Quaternion.identity);
            currentOrb = currentSpell;
            currentSpell.transform.forward = directionOfShot.normalized;

            currentSpell.GetComponent<Rigidbody>().AddForce(directionOfShot.normalized * 0.3f, ForceMode.Impulse);

            anim.SetTrigger("IsAttacking");

            sound.PlayOneShot(sounds[0], 0.5f);
            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;
            }
        }
    }

    private void Heal()
    {
        if(currentHeal) Destroy(currentHeal);

        faith -= 4;
        UpdateBar(faithBar, faith, maxFaith);
        sound.PlayOneShot(sounds[8]);
        currentHeal = Instantiate(zone, transform.position, Quaternion.identity);
    }
  
    //#############################################################
    // MISC
    //#############################################################

    private void CheckMana()
    {
        if(sprinting && mana < .1)
        {
            sprinting = false;
            sprintCircle.SetActive(false);
            moveSpeed = walkSpeed;
        }
    }

    private void RegenMana()
    {
        if (sprinting && mana > 0) mana -= Time.deltaTime * 2;

        if (!sprinting && mana < maxMana) mana += Time.deltaTime;
        UpdateBar(manaBar, mana, maxMana);
    }

    private void RegenFaith()
    {
        if(moveDirection.magnitude < 0.01 && faith < maxFaith)
        {
            faith += Time.deltaTime * faithRegen; 
        }
        UpdateBar(faithBar, faith, maxFaith);
    }

    private void RageDecay()
    {
        if(rage > 0)
        {
            rage -= 1f * Time.deltaTime;
            UpdateBar(rageBar, rage, rageTime);
            if (rage < 0f)
            {
                rage = 0f;
                enraged = false;
            }
        }
    }

    public void ResetEnrage()
    {
        sound.PlayOneShot(sounds[7]);
        Invoke(nameof(AResetEnrage), 5f);
    }

    public void AResetEnrage()
    {
        canEnrage = true;
    }

    public void TakeDamage(int damage)
    {
        if (!dead)
        { 
            health -= damage * (1 - damageReduction);
            UpdateBar(healthBar, health, maxHealth);
            if (health < 0)
            {
                Die();
            }
        }
    }

    public void DrainHealth(float damage)
    {
        if (!dead)
        {
            health -= Time.deltaTime * damage * (1 - damageReduction);
            UpdateBar(healthBar, health, maxHealth);

            if (health < 0)
            {
                Die();
            }
        }
    }

    public void Heal(int healing)
    {
        if (!dead)
        {
            health += healing;
            if (health > maxHealth) health = maxHealth;
            UpdateBar(healthBar, health, maxHealth);
        }
    }

    public void HealOverTime(float healing)
    {
        if(!dead)
        {
            health += Time.deltaTime * healing;
            if(health> maxHealth) health = maxHealth;
            UpdateBar(healthBar, health, maxHealth);
        }
    }

    public void SwapLilline()
    {
        if(currentChar != Character.Lilline)
        {
            if (currentChar == Character.Arden) chosenSpell = lastSpell;
            currentChar = Character.Lilline;
            anim = lilAnim;
            lillineModel.SetActive(true);
            ardenModel.SetActive(false);
            feliciaModel.SetActive(false);
            manaBarObj.SetActive(true);
            rageBarObj.SetActive(false);
            faithBarObj.SetActive(false);
        }
    }

    public void SwapArden()
    {
        if (ardUnlocked && currentChar != Character.Arden)
        {
            lastSpell = chosenSpell;
            chosenSpell = 0;
            currentChar = Character.Arden;
            anim = ardAnim;
            lillineModel.SetActive(false);
            ardenModel.SetActive(true);
            feliciaModel.SetActive(false);
            manaBarObj.SetActive(false);
            rageBarObj.SetActive(true);
            faithBarObj.SetActive(false);
        }
    }

    public void SwapFelicia()
    {
        if (felUnlocked && currentChar != Character.Felicia)
        {
            if (currentChar == Character.Arden) chosenSpell = lastSpell;
            currentChar = Character.Felicia;
            anim = felAnim;
            lillineModel.SetActive(false);
            ardenModel.SetActive(false);
            feliciaModel.SetActive(true);
            manaBarObj.SetActive(false);
            rageBarObj.SetActive(false);
            faithBarObj.SetActive(true);
        }
    }

    public void Respawn()
    {
        respawnMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        dead = false;
        health = maxHealth;
        mana = maxMana;
        rage = 0;
        faith = maxFaith;
        SaveManager.GetInstance().Load();
        anim.SetBool("Die", dead);
        UpdateBar(healthBar, health, maxHealth);
        UpdateBar(manaBar, mana, maxMana);
        UpdateBar(rageBar, rage, rageTime);
        UpdateBar(faithBar, faith, maxFaith);
    }

    void Die()
    {
        dead = true;
        respawnMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        anim.SetBool("Die",dead);
    }

    public void UpdateBar(Slider barToUpdate, float currentValue, float maxValue)
    {
        barToUpdate.value = currentValue/maxValue;
    }

    //#############################################################
    // END MISC
    //#############################################################
}
