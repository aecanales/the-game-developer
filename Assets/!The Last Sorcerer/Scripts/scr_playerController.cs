using System.Collections;
using UnityEngine;

public class scr_playerController : MonoBehaviour
{

    Vector3 velocity;
    public ParticleSpawner Pspawner;
    public Rigidbody body;
    public Animator anim;
    public float mSpd;
    public float dashSpd;
    public float dashCooldown;
    public GameObject pitCheckObj;
    public float defaultSpd;
    public float dTime = 0.5f;
    public float health;
    public bool dashing;
    public bool noMove;
    public bool stunned;
    public Vector3 rotationSetting;

    //public GameObject firePointU;
    //public GameObject firePointD;
    //public GameObject firePointR;
    //public GameObject firePointL;
    public GameObject activeFirePoint;
    public GameObject equippedSpell;
    public SpellType equippedEffect;
    public GameObject equippedFamiliar;
    public GameObject spell2;
    public GameObject spell1;
    public GameObject currentAttack;
    public GameObject charmedThrall;
    public GameObject[] currentFamiliars;
    public GameObject swipe, beam, shield;
    public bool spellOnCooldown;
    public float cooldownTime;

    public GameObject familiar1, familiar2, familiar3;
    public bool openFamiliarSlot = true;
    private Transform spellSpawn;

    public enum SpellType
    {
        Push,
        Pull,
        Charm, //Added new spell type
        Slash,
        Freeze,
        Summon
    }

    public SpellType spell1Type;
    public SpellType spell2Type;
    // New method for choosing spell types

    // Start is called before the first frame update
    void Start()
    {
        //activeFirePoint = firePointU;
        body = GetComponent<Rigidbody>();
        rotationSetting = new Vector3(0, 0, 0f);
    }



    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.z = Input.GetAxisRaw("Vertical"); //We move on X and Z, not Y. Result of moving from 2 to 3 dimensions
        if (Input.GetKeyDown(KeyCode.Space) && currentAttack == null && !stunned) { Dash(); }

        if (velocity.x > 0)
        {
            //activeFirePoint = firePointR;
            rotationSetting = new Vector3(0, 90f, 0); //In 3D we rotate on the Y, not Z
        }
        else if (velocity.x < 0)
        {
            //activeFirePoint = firePointL;
            rotationSetting = new Vector3(0, -90f, 0);
        }
        else if (velocity.z > 0 && velocity.x == 0)
        {
            //activeFirePoint = firePointU;
            rotationSetting = new Vector3(0, 0, 0);
        }
        else if (velocity.z < 0 && velocity.x == 0)
        {
            //activeFirePoint = firePointD;
            rotationSetting = new Vector3(0, 180f, 0f);
        }
        if (velocity.z != 0 || velocity.x != 0) { anim.SetBool("MOVE", true); }
        else { anim.SetBool("MOVE", false); }
        if (dashing) { anim.SetBool("DASH", true); }
        else { anim.SetBool("DASH", false); }



        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    if (equippedSpell == spellA) { equippedSpell = spellB; }
        //    else if (equippedSpell == spellB) { equippedSpell = spellA; }
        //}
        if (Input.GetKeyDown(KeyCode.J) && !dashing && !stunned)
        {
            if (spell1 == gameObject) { gameObject.GetComponent<scr_health>().health = 0; }
            else
            {
                equippedSpell = spell1;
                equippedEffect = spell1Type;
                anim.SetFloat("Blend", Random.Range(0, 1));
                anim.SetTrigger("ATTACK");
                //Attack();
            }
        }

        if (Input.GetKeyDown(KeyCode.I) && !dashing && !stunned)
        {
            if (spell2 == gameObject) { gameObject.GetComponent<scr_health>().health = 0; }
            else
            {
                equippedSpell = spell2;
                equippedEffect = spell2Type;
                anim.SetFloat("Blend", Random.Range(0, 1));
                anim.SetTrigger("ATTACK");
                //Attack();
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && !dashing && !stunned) { Summon(equippedFamiliar); }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchSpell1ToSwipe();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchSpell1ToBeam();
        }

        if (currentAttack != null && !noMove)
        {
            mSpd = 0f;
            //if (currentAttack.GetComponent<scr_spells>().PushSpell) { mSpd = 0f; }
            if (currentAttack.GetComponent<scr_spells>().PullSpell) { mSpd = defaultSpd; }
        }
        else if (currentAttack == null && !noMove) { mSpd = defaultSpd; }
        if (noMove) { mSpd = 0f; }
        else {  mSpd = defaultSpd; }

        if (currentAttack != null)
        {
            if (currentAttack.GetComponent<scr_spells>().PullSpell)
            {
                currentAttack.gameObject.transform.position = activeFirePoint.transform.position;
                if (Input.GetKeyUp(KeyCode.I))
                {
                    Destroy(currentAttack.gameObject);
                    currentAttack = null;
                }
                else if (Input.GetKeyUp(KeyCode.J))
                {
                    Destroy(currentAttack.gameObject);
                    currentAttack = null;
                }
            }
        }
        if (PitCheck() && !dashing) //Checks for pits when walking so we don't fall and die
        {
            body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        }
        else if (!PitCheck() && !dashing) //But allows us to dash over them
        {
            body.constraints = RigidbodyConstraints.None;
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        //if (Input.GetKeyDown(KeyCode.I)) { SwitchSpell(equippedSpell); }
        if (currentAttack != null && equippedSpell == shield) { gameObject.GetComponent<scr_health>().invincible = true; }
        else { gameObject.GetComponent<scr_health>().invincible = false; }
        if (health <= 0f) { Death(); }
    }

    public void EnableMovement() { noMove = false; }
    public void DisableMovement() { noMove = true; }
    public void StunMe() { stunned = true; }
    public void DeStunMe() { stunned = false; }


    public void ChangeFirstSpellEffect(int NewType)
    {
        spell1Type = (SpellType)NewType;
    }

    public void ChangeSecondSpellEffect(int NewType)
    {
        spell2Type = (SpellType)NewType;
    }

    public void ToggleObject(GameObject subject) 
    {
        if (subject != null)
        {
            if (subject.activeSelf)
            {
                subject.SetActive(false);
            }
            else if (!subject.activeSelf) 
            { 
                subject.SetActive(true);
            }
        }
    }

    public void SwitchSpell1ToSwipe()
    {
        spell1 = swipe;
    }
    public void SwitchSpell1ToBeam()
    {
        spell1 = beam;
    }
    public void SwitchSpell1ToShield()
    {
        spell1 = shield;
    }
    public void SwitchSpell1ToSelf()
    {
        spell1 = gameObject;
    }
    public void SwitchSpell2ToSwipe()
    {
        spell2 = swipe;
    }
    public void SwitchSpell2ToBeam()
    {
        spell2 = beam;
    }
    public void SwitchSpell2ToShield()
    {
        spell2 = shield;
    }
    public void SwitchSpell2ToSelf()
    {
        spell2 = gameObject;
    }
    public void SlotMyFamiliar(GameObject familiar)
    {

        
            if (familiar1 == null)
            {
                familiar1 = familiar;
                return;
            }
            else if (familiar2 == null)
            {
                familiar2 = familiar;
                return;
            }
            else if (familiar3 == null)
            {
                familiar3 = familiar;
                return;
            }
            else
            {
                return;
            }
        

    }

    public bool CheckForOpenFamiliarSlots()
    {

            if (familiar1 == null || familiar2 == null || familiar3 == null)
            {
                return true;
            }
            else
            {
                return false;
            }

    }

    public void DestroyFamiliar()
    {
        if (familiar1 != null)
        {
            Destroy(familiar1);
            familiar1 = null;
            return;
        }
        else if (familiar2 != null)
        {
            Destroy(familiar2);
            familiar2 = null;
            return;
        }
        else if (familiar3 != null)
        {
            Destroy(familiar1);
            familiar3 =null;
            return;
        }
        else
        {
            return;
        }
    }

    public void Attack()
    {
        var spellShape = equippedSpell;
        var type = equippedEffect;
        if (currentAttack == null)
        {
            if (spellShape == shield)
            {
                spellSpawn = gameObject.transform;
            }
            else
            {
                spellSpawn = activeFirePoint.transform;
            }
            var copy = Instantiate(spellShape, spellSpawn.position, Quaternion.identity);
            copy.transform.eulerAngles = rotationSetting;
            currentAttack = copy;
            if (type == SpellType.Push)
            {
                var copyScript = copy.GetComponent<scr_spells>();
                copyScript.PushSpell = true;
                copyScript.PullSpell = false;
                copyScript.CharmSpell = false; //HERE...
                copyScript.SlashSpell = false;
                copyScript.FreezeSpell = false;
                if (spellShape == swipe)
                {
                    Pspawner.SpawnParticle(ParticleSpawner.ParticleType.PushSwipe);
                }
                else if (spellShape == beam)
                {
                    Pspawner.SpawnParticle(ParticleSpawner.ParticleType.PushBeam);
                }
                //else if (spellShape == shield) { }
            }
            else if (type == SpellType.Pull)
            {
                var copyScript = copy.GetComponent<scr_spells>();
                copyScript.PushSpell = false;
                copyScript.PullSpell = true;
                copyScript.CharmSpell = false; //HERE...
                copyScript.SlashSpell = false;
                copyScript.FreezeSpell = false;
                if (spellShape == swipe)
                {
                    Pspawner.SpawnParticle(ParticleSpawner.ParticleType.PullSwipe);
                }
                else if (spellShape == beam)
                {
                    Pspawner.SpawnParticle(ParticleSpawner.ParticleType.PullBeam);
                }

            }
            else if (type == SpellType.Charm) //AND HERE
            {
                var copyScript = copy.GetComponent<scr_spells>();
                copyScript.PushSpell = false;
                copyScript.PullSpell = false;
                copyScript.CharmSpell = true;
                copyScript.SlashSpell = false;
                copyScript.FreezeSpell = false;
            }
            else if (type == SpellType.Slash) 
            {
                var copyScript = copy.GetComponent<scr_spells>();
                copyScript.PushSpell = false;
                copyScript.PullSpell = false;
                copyScript.CharmSpell = false;
                copyScript.SlashSpell = true;
                copyScript.FreezeSpell = false;
            }
            else if (type == SpellType.Freeze)
            {
                var copyScript = copy.GetComponent<scr_spells>();
                copyScript.PushSpell = false;
                copyScript.PullSpell = false;
                copyScript.CharmSpell = false;
                copyScript.SlashSpell = false;
                copyScript.FreezeSpell = true;
            }
            if (!copy.GetComponent<scr_spells>().PullSpell)
            {
                //Vector3 spellScale = copy.transform.localScale;
                //copy.transform.localScale = new Vector3(spellScale.x + spellScale.x, spellScale.y + spellScale.y, spellScale.z); //enlarges the spell, code may be useful later
                Destroy(copy, dTime);
            }

        }
        // Push/Pull combo
        else if (currentAttack != null 
            && currentAttack.GetComponent<scr_spells>().PullSpell
            && type == SpellType.Push
            && currentAttack.GetComponent<scr_spells>().pulled != null) 
        {
            currentAttack.GetComponent<scr_spells>().Push(currentAttack.GetComponent<scr_spells>().pulled);
            Destroy(currentAttack.gameObject);
            currentAttack = null;
        }
    }

    void Summon(GameObject familiar) //We need a way to keep track of how many summons we have, and to know when they die. Check each familiar slot and proceed if empty?
                                     //How do we decide which to assign the spawn to?
    {
        if (!spellOnCooldown) 
        {
            if (!CheckForOpenFamiliarSlots()) { DestroyFamiliar(); }
            var copy = Instantiate(familiar, activeFirePoint.transform.position, Quaternion.identity);
            if (copy.GetComponent<enemyAI_Script>() != null) 
            {
                Debug.Log("Summoned enemy");
                copy.GetComponent<enemyAI_Script>().whatIsPlayer = LayerMask.GetMask("Enemies");
                copy.GetComponent<enemyAI_Script>().bodyguard = true;
                copy.GetComponent<enemyAI_Script>().ward = this.gameObject;
                copy.layer = gameObject.layer;
            }
            SlotMyFamiliar(copy);
            spellOnCooldown = true;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }
    }

    void ResetCooldown()
    {
        spellOnCooldown = false;
    }

    void Dash()
    {
        dashing = true;
        body.constraints = RigidbodyConstraints.FreezePositionY;
        Debug.Log("Dash");
        //body.MovePosition(body.position + velocity * dashSpd * Time.deltaTime);
        //body.MovePosition(body.position);

        var direction = activeFirePoint.transform.position - body.transform.position;
        body.AddForce(direction.normalized * dashSpd, ForceMode.Impulse);
        Invoke(nameof(ResetDash), dashCooldown);
        //body.velocity = Vector3.zero;
        //body.angularVelocity = Vector3.zero;

        //body.MovePosition(activeFirePoint.transform.position );
    }

    private void ResetDash()
    {
        body.constraints = RigidbodyConstraints.FreezePosition;
        body.constraints = RigidbodyConstraints.None;
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        dashing = false;
    }

    private bool PitCheck() //Checks for pits. Uses default layer for now. CHANGE IF WE MAKE A GROUND LAYER.
    {
        LayerMask layerMask = LayerMask.GetMask("Default");

        if (Physics.Raycast(pitCheckObj.transform.position, -Vector3.up, 8f, layerMask))
        {
            return false;
        }
        else 
        {
            //Debug.Log("OVER PIT");
            return true;
        }
    }

    private void SwitchSpell(GameObject currentSpell) // not currently in use
    {
        if (currentSpell == spell1){ currentSpell = spell2; }
        else if (currentSpell == spell2){ currentSpell = spell1 ; }
    }

    void Death()
    {
        Debug.Log("YOU DIED");
        // Code for death, reset scene?
    }
    public void Freeze(GameObject frozenObject)
    {
        if(frozenObject.GetComponent<scr_health>() != null)
        {
            frozenObject.GetComponent<scr_health>().invincible = true;
        }
        if (frozenObject.GetComponent<enemyAI_Script>() != null)
        {
            Rigidbody body = frozenObject.GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeAll;
            frozenObject.GetComponent<enemyAI_Script>().agent.enabled = false;
            frozenObject.GetComponent<enemyAI_Script>().enabled = false;
            StartCoroutine(UnfreezeTarget(frozenObject));
        }
        else if (frozenObject.GetComponent<enemyAI_Script>() == null)
        {
            Rigidbody body = frozenObject.GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(UnfreezeTarget(frozenObject));
        }
    }
    public IEnumerator UnfreezeTarget(GameObject stunnedObject) 
    {
        Debug.Log("Running DeStun...");
        yield return new WaitForSeconds(3);
        Debug.Log("DeStun wait time complete");
        if (stunnedObject.GetComponent<scr_health>() != null)
        {
            stunnedObject.GetComponent<scr_health>().invincible = false;
        }
        if (stunnedObject.GetComponent<enemyAI_Script>() != null)
        {
            stunnedObject.GetComponent<enemyAI_Script>().enabled = true;
            stunnedObject.GetComponent<enemyAI_Script>().agent.enabled = true;

            Rigidbody body = stunnedObject.GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.None;
            body.constraints = RigidbodyConstraints.FreezeRotationX;
            body.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
        else if (stunnedObject.GetComponent<enemyAI_Script>() == null)
        {
            Rigidbody body = stunnedObject.GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.None;
        }
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + velocity * mSpd * Time.fixedDeltaTime);
        transform.eulerAngles = rotationSetting;

    }
}

