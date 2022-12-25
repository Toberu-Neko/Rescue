using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class TestBossController : MonoBehaviour
{
    public Transform attackDetector;
    public Vector3 attackDetectorSize;
    public Vector3 attackRange;
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    public LayerMask attackIgnoreLayer;
    public GameObject attackSphare;

    private Animator animator;
    [SerializeField] private GameObject knockBackSphare;
    private BossKnockback bossKnockback;
    [SerializeField] private float knockbackMaxSacle;
    private Collider knockBackSphareCollider;
    private GameObject bossHealthBar;
    private Rigidbody playerRig;
    private ForceMotionNew forceMotion;
    private TestBossStates testBossStates;

    public int attackCount;
    public int attackDamage;

    public float attackPrefabRadius;
    [SerializeField] private float attackCD;

    public float damageCount;
    bool playerInAttackRange;
    private bool attackAble;

    void Start()
    {
        //attackCD = attackSphare.GetComponent<TestBossAttack>().attackPrepareTime;
        playerRig = PlayerManager.instance.player.GetComponent<Rigidbody>();
        forceMotion = PlayerManager.instance.player.GetComponent<ForceMotionNew>();
        testBossStates = gameObject.GetComponent<TestBossStates>();
        bossKnockback = knockBackSphare.GetComponent<BossKnockback>();
        animator = transform.Find("Design/BlueBoss").gameObject.GetComponent<Animator>();

        bossHealthBar = UIManager.instance.UI.transform.Find("HUD/BossHealthBar").gameObject;
        knockBackSphare.SetActive(false);
        playerInAttackRange = false;
        attackAble = true;
        damageCount = 0;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;

        if(Physics.CheckBox(attackDetector.position, attackDetectorSize, Quaternion.identity, playerLayer))
        {
            playerInAttackRange = true;
        }else
        {
            playerInAttackRange = false;
        }

        if (damageCount >= testBossStates.maxHealth / 4f)
        {
            attackAble = false;
            damageCount = 0;
            StopAllCoroutines();
            //StopCoroutine(WaterNormalAttack());
            StartCoroutine(MaxHealthMinuse25());
        }

        if (playerInAttackRange)
        {
            testBossStates.HealthBarActive();

            if (attackAble)
            {
                //Debug.Log(attackAble);
                StartCoroutine(WaterNormalAttack());
            }
        }
        else if (!playerInAttackRange)
        {
            testBossStates.HealthBarDeactive();
        }
        



    }
    IEnumerator MaxHealthMinuse25()
    {
        testBossStates.invincible = true;
        testBossStates.ChangeAttackedMaterial();
        Vector3 _originScale = knockBackSphare.transform.localScale;
        knockBackSphare.SetActive(true);
        while (knockBackSphare.transform.localScale.y < knockbackMaxSacle-0.8f)
        {
            knockBackSphare.transform.localScale = Vector3.Lerp(knockBackSphare.transform.localScale, new Vector3(knockbackMaxSacle, knockbackMaxSacle, knockbackMaxSacle), Time.deltaTime * 8f); ;
            //Debug.Log(knockBackSphare.transform.localScale.y);
            yield return new WaitForSeconds(.1f) ;
        }
        animator.SetBool("Attack", true);
        //Debug.Log("Set bool true");
        yield return new WaitForSeconds(1.15f);
        animator.SetBool("Attack", false);
        bossKnockback.dealDamage = true;
        /*if(Physics.CheckSphere(knockBackSphare.transform.position, knockbackMaxSacle - 0.8f, playerLayer))
        {
            Debug.Log("In  range");
            forceMotion.speedControlAble = false;
            playerRig.AddForce(((playerRig.transform.position + Vector3.down * .5f) - knockBackSphare.transform.position).normalized * 80f, ForceMode.Impulse);
            Invoke(nameof(speedControlAbleTrue), 1f);
        }*/
        yield return new WaitForSeconds(.03f);
        bossKnockback.dealDamage = false;
        testBossStates.ChangeNormalMaterial();
        knockBackSphare.transform.localScale = _originScale;
        knockBackSphare.SetActive(false);
        attackAble = true;
        testBossStates.invincible = false;
    }

    IEnumerator WaterNormalAttack()
    {
        attackAble = false;
        //find attack point
        for (int i = 0; i < attackCount; i++)
        {
            Vector3 attackPoint = new Vector3(Random.Range(attackDetector.position.x - attackRange.x, attackDetector.position.x + attackRange.x), attackDetector.position.y + attackRange.y, Random.Range(attackDetector.position.z - attackRange.z, attackDetector.position.z + attackRange.z));

            while (!Physics.Raycast(attackPoint, Vector3.down, out _, 3000f, groundLayer))
            {
                attackPoint = new Vector3(Random.Range(attackDetector.position.x - attackRange.x, attackDetector.position.x + attackRange.x), attackDetector.position.y + attackRange.y, Random.Range(attackDetector.position.z - attackRange.z, attackDetector.position.z + attackRange.z));
            }
            if (Physics.Raycast(attackPoint, Vector3.down, out RaycastHit hit, 3000f, groundLayer))
            {
                GameObject bossAttack = Instantiate(attackSphare, attackPoint, Quaternion.identity);
                TestBossAttack testBossAttack = bossAttack.GetComponent<TestBossAttack>();
                testBossAttack.attackRadius = attackPrefabRadius /2;
                testBossAttack.attackDamage = attackDamage;
                bossAttack.transform.localScale *= attackPrefabRadius;
                
                attackPoint.y = hit.point.y;
                bossAttack.transform.SetParent(this.transform);
            }
            yield return new WaitForSeconds(0.001f) ;
        }
        if (attackCD > 0)
        {
            yield return new WaitForSeconds(attackCD);
            attackAble = true;
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
            attackAble = true;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawCube(transform.position, attackDetectorSize);

        if (attackDetector == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackDetector.position, attackDetectorSize *2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackDetector.position, attackRange*2);

    }
}
