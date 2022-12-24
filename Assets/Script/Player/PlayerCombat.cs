using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Com.Neko.ThreeDGameProjecct;

public class PlayerCombat : MonoBehaviour
{
    //https://youtu.be/sPiVz1k-fEs
    //public Animator animator;

    [Header("近戰數值")]
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;

    [Header("遠程投擲")]
    private float fireRate;
    private float throwCooldown;
    private float throwStartCountDownTime;
    private float maxThrowForce;
    private float minThrowForce;
    private float maxRightThrowForce;
    private float minRightThrowForce;
    private float throwUpwardForce;

    bool throwAble;

    [Header("近戰物件")]
    public Transform attackDetector;
    public LayerMask enemyLayers;
    public GameObject debugSphare;
    public GameObject attackParticle;

    [Header("遠程物件")]
    public Transform throwAttackPoint;
    public GameObject objectToThrow;

    [Header("UI物件")]
    [SerializeField] private Image skillCdCircle;
    [SerializeField] private Image waterCdCircle;
    //[SerializeField] private UnityEvent unityEvent;

    [Header("按鍵設定")]
    public KeyCode throwAttackKey = KeyCode.Mouse0;
    public KeyCode basicAttackKey = KeyCode.Mouse0;
    //public KeyCode 
    public LayerMask attackIgnoreLayer;

    //Private
    private GameObject stoneParticle;
    private Animator animator;
    private Transform mainCameraTransform;
    private Transform playerTransform;
    private PlayerStates playerStates;
    private ForceMotionNew forceMotionNew;
    private GameObject hitParticle;
    private float aimDetectRange;
    bool refillAble;
    bool attackAble;
    bool throwing;
    [HideInInspector]public bool throwColldownActivated;

    bool inWaterRange;

    int previousSkillSlot;
    float waterAbleTimer;
    float attackTimer;
    [HideInInspector] public float throwTimer;
    float notThrowingTimer;
    float overHeatCD;





    void Start()
    {
        stoneParticle = transform.Find("Particle/Rock").gameObject;
        hitParticle = ParticleManager.instance.hitParticle;
        animator = transform.Find("CatPlayerOBJ(Rotation here)/Cat").gameObject.GetComponent<Animator>();
        mainCameraTransform = PlayerManager.instance.mainCamera.transform;
        playerTransform = PlayerManager.instance.player.transform;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        forceMotionNew = playerStates.gameObject.GetComponent<ForceMotionNew>();
        skillCdCircle = UIManager.instance.UI.transform.Find("HUD/SkillCDCircle").gameObject.GetComponent<Image>();
        waterCdCircle = UIManager.instance.UI.transform.Find("HUD/WaterCDCircle").gameObject.GetComponent<Image>();
        waterCdCircle.fillAmount = 0;
        SelectSkill(0);
        skillCdCircle.color = Color.black;
        waterCdCircle.enabled = false;
        skillCdCircle.enabled = false;

        inWaterRange = false;
        attackAble = true;
        throwAble = true;
        throwing = false;
        throwColldownActivated = false; 
        attackTimer = 0;
        throwTimer = 0;
        notThrowingTimer = 0;
        attackParticle.SetActive(false);
        debugSphare.SetActive(false);
        //Vector3 _debugStartScale = debugSphare.transform.localScale;
        debugSphare.transform.localScale *= attackRange;
    }

    void Update()
    {
        #region 近戰CD與Input
        if (Input.GetKeyDown(basicAttackKey) && Cursor.lockState == CursorLockMode.Locked && attackAble && !playerStates.isAiming)
        {
            attackAble = false;
            Attack();
        }
        if (!attackAble)
        {
            attackTimer += Time.deltaTime;
        }
        if (attackTimer > attackCooldown)
        {
            attackTimer = 0;
            attackAble = true;
        }
        #endregion
        
        if (throwCooldown < 9998 && !throwColldownActivated && Input.GetKey(throwAttackKey) && Cursor.lockState == CursorLockMode.Locked && throwAble && playerStates.isAiming && !inWaterRange)
        {
            Throw();
            skillCdCircle.enabled = true;
        }
        //else if(Input.GetKeyUp(throwAttackKey) || !playerStates.isAiming)
        //throwing = false;
        if (playerStates.skillSlot == -1)
        {
            if (throwTimer >= throwCooldown)
            {
                SelectSkill(previousSkillSlot);
                waterCdCircle.enabled = false;
                waterCdCircle.fillAmount = 0;
            }
            if (waterCdCircle.enabled)
            {
                waterCdCircle.fillAmount = (throwCooldown - throwTimer) / throwCooldown;
            }
        }
        else
            ThrowCDRefill();
    }
    void ThrowCDRefill()
    {
        if (throwColldownActivated)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer <= 0)
            {
                throwColldownActivated = false;
                skillCdCircle.color = Color.black;
            }
        }

        if (!throwing && !throwColldownActivated && refillAble)
        {
            if (notThrowingTimer <= throwStartCountDownTime)
                notThrowingTimer += Time.deltaTime;
            if (notThrowingTimer >= throwStartCountDownTime)
            {
                if (throwTimer <= 0)
                {
                    skillCdCircle.enabled = false;
                    throwTimer = 0;
                }
                else
                    throwTimer -= Time.deltaTime;
            }

        }
        if (throwTimer >= throwCooldown && !throwColldownActivated)
        {
            throwColldownActivated = true;
            skillCdCircle.color = Color.red;
            throwTimer = throwCooldown + overHeatCD;
        }

        if (skillCdCircle.enabled)
        {
            skillCdCircle.fillAmount = (throwCooldown - throwTimer) / throwCooldown;
        }
    }
    void Attack()
    {
        //Play an attack animation
        forceMotionNew.standingTimer = 0;
        AudioManager.instance.Play("PlayerAttack");
        StopAllCoroutines();
        //animator.SetBool("Attack", true);
        StartCoroutine(DebugSphareActive());

        //Detect enemies in range or not
        Collider[] hitEnemies =  Physics.OverlapSphere(attackDetector.position, attackRange, enemyLayers); 

        //Apply damage
        foreach(Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                enemy.GetComponent<EnemyStates>().EnemyTakeDamage(attackDamage);
            }

            if (enemy.gameObject.layer == LayerMask.NameToLayer("Bosses"))
            {
                enemy.GetComponent<TestBossStates>().BossTakeDamage(attackDamage);
            }
            GameObject particle = Instantiate(hitParticle, attackDetector.transform.position, Quaternion.identity);
            Destroy(particle, 1f);
        }
    }
    void Throw()
    {
        throwTimer += fireRate;
        notThrowingTimer = 0;

        throwAble = false;
        throwing = true;
        //spawn Obj 
        GameObject throwObj = Instantiate(objectToThrow, throwAttackPoint.position, playerTransform.rotation);

        Rigidbody throwObjRig = throwObj.GetComponent<Rigidbody>();


        Vector3 forceDirection = mainCameraTransform.forward;
        RaycastHit hit;
        if (Physics.Raycast(mainCameraTransform.position, mainCameraTransform.forward, out hit, aimDetectRange, ~attackIgnoreLayer))
        {
            forceDirection = (hit.point - throwAttackPoint.position).normalized;
        }
        //add force
        Vector3 forceToAdd = forceDirection * Random.Range(minThrowForce, maxThrowForce) + transform.up * throwUpwardForce + transform.right * Random.Range(minRightThrowForce, maxRightThrowForce); ;

        throwObjRig.AddForce(forceToAdd, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), fireRate);
    }
    void ResetThrow()
    {
        throwAble = true;
        throwing = false;
    }

    IEnumerator DebugSphareActive()
    {
        animator.SetBool("Attack", true);
        attackParticle.SetActive(true);
        yield return new WaitForSeconds(attackCooldown);
        attackParticle.SetActive(false);
        animator.SetBool("Attack", false);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackDetector == null)
            return;

        Gizmos.DrawWireSphere(attackDetector.position, attackRange);
    }

    public void SelectSkill(int skillSlot)
    {
        previousSkillSlot = playerStates.skillSlot;
        if (skillSlot == -1)
        {
            skillCdCircle.fillAmount = 1;
            skillCdCircle.enabled = false;
            objectToThrow = SkillManager.instance.waterTemporary;
        }
        if(skillSlot == 0)
        {
            objectToThrow = SkillManager.instance.noSkill;
        }
        if (skillSlot == 1 && playerStates.throwStoneAble)
        {
            objectToThrow = SkillManager.instance.testWater;
            stoneParticle.SetActive(true);
        }
        if (skillSlot == 2 && playerStates.throwFireAble)
        {
            objectToThrow = SkillManager.instance.fire;
            stoneParticle.SetActive(false);
        }
        if (skillSlot == 3 && playerStates.throwDebugAble)
        {
            objectToThrow = SkillManager.instance.debug;
            stoneParticle.SetActive(true);
        }
        playerStates.skillSlot = skillSlot;
        throwTimer = 0;
        //throwColldownActivated = false;


        //objectToThrow.GetComponent<SkillState>().skill = objectToThrow.GetComponent<Skill>();
        Skill skill;
        skill = objectToThrow.GetComponent<SkillState>().skill;
        refillAble = !skill.isTemporarySkill;
        maxThrowForce = skill.maxThrowForce;
        minThrowForce = skill.minThrowForce;
        minRightThrowForce= skill.minRightThrowForce;
        maxRightThrowForce= skill.maxRightThrowForce;
        throwUpwardForce = skill.throwUpwardForce;
        fireRate = skill.fireRate;
        aimDetectRange = skill.aimDetectRange;
        throwCooldown = skill.cooldown;
        throwStartCountDownTime = skill.startCountDownTime;
        overHeatCD = skill.overHeatCD;
        ResetThrow();
    }
}
