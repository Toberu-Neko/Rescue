using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("°»´ú")]
    public float lookRadius;
    public float maxAttackAngle;
    public Transform attackDetector;
    [SerializeField]private Transform playerDetector;
    public LayerMask playerLayer, ground;
    public GameObject debugSphare;
    public GameObject DamageSphare;

    [Header("§ðÀ»")]
    public float attackRange;
    public float attackChargeTime;
    public float attackColldown;
    public int attackDamage;
    public float walkPointRange;
    public float maxPatrolDistance;
    private Vector3 startPosition;
    private Vector3 walkPoint;

    [SerializeField] GameObject attackParticle;
    bool walkPointSet;
    GameObject hitParticle;
    Animator animator;
    PlayerStates playerStates;
    Transform target;
    NavMeshAgent agent;
    EnemyStates enemyStates;
    bool turnAble;
    bool attackComplete;
    bool walkAble;
    float searchWalkPointResetTimer;

    void Start()
    {
        attackParticle.SetActive(false);
        searchWalkPointResetTimer = 0;
        turnAble = true;
        walkAble = true;
        enemyStates = gameObject.GetComponent<EnemyStates>();
        hitParticle = ParticleManager.instance.hitParticle;
        animator = gameObject.transform.Find("¼Ò«¬/EM").gameObject.GetComponent<Animator>();
        startPosition = transform.position;
        attackComplete = true;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStates.died)
        {
            StopAllCoroutines();
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        Vector3 _attackRange = new Vector3(0, 0.5f, 1) * attackRange + new Vector3(1.5f, 0, 0);
        Vector3 playerDirection = (playerStates.transform.position - playerDetector.position).normalized;
        if (attackComplete && Physics.Raycast(playerDetector.position, playerDirection, out _, (attackRange - playerDetector.localPosition.z) * 2f, playerLayer) && Vector3.Angle(playerDirection, transform.forward) < maxAttackAngle)
        {
            //Debug.Log(Vector3.Distance(playerStates.transform.position, playerDetector.position));
            Attack();
        }
        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance)
            {
                //attack
                //Face target
                FaceTarget();
            }
        }
        else if(distance > lookRadius)
        {
            if (!walkPointSet)
            {
                SearchWalkPoint();
            }
            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
                searchWalkPointResetTimer += Time.deltaTime;
            }
            if(searchWalkPointResetTimer >= 10)
            {
                SearchWalkPoint();
            }
            if (!walkAble)
            {
                //agent.isStopped = true;
            }
            else if (walkAble)
            {
                //agent.isStopped = false;
            }
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 5f)
            {
                //StopAllCoroutines();
                //StartCoroutine(findNewMovePointDelay());
                walkPointSet = false;
            }
        }
        void SearchWalkPoint()
        {
            
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            {
                searchWalkPointResetTimer = 0;
                walkPointSet = true;
                Vector3 distanceToStartPosition = walkPoint - startPosition;
                if (distanceToStartPosition.magnitude > maxPatrolDistance)
                {
                    walkPoint = startPosition;
                }
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        if(turnAble)
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 13f);
    }
    void Attack()
    {
        StopAllCoroutines();
        StartCoroutine(debugSphareActive());
    }
    IEnumerator debugSphareActive()
    {
        attackComplete = false;
        walkAble=false;
        turnAble = false;
        agent.isStopped = true;
        animator.SetBool("Attack", true);
        animator.SetFloat("ChargeSpeed", 1 / attackChargeTime);
        attackParticle.SetActive(true);
        yield return new WaitForSeconds(attackChargeTime);
        
        Vector3 _attackRange = new Vector3(0, 0.5f, 1) * attackRange + new Vector3(1.5f, 0, 0);
        _attackRange *= 1f;
        if (Physics.CheckBox(attackDetector.position, _attackRange, transform.rotation, playerLayer))
        {
            playerStates.TakeDamage(attackDamage);
            GameObject particle = Instantiate(hitParticle, playerStates.transform.position, Quaternion.identity);
            Destroy(particle, 1f);
        }
        yield return new WaitForSeconds(1.3333333f / 12f);
        attackParticle.SetActive(false);
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(1.3333333f / 12f);
        yield return new WaitForSeconds(attackColldown);
        attackComplete = true;
        walkAble = true;
        turnAble = true;
        if(!enemyStates.died)
        agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        if (attackDetector == null)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(attackDetector.position, new Vector3(0, 0.5f, 1) * attackRange + new Vector3(1.5f, 0, 0));
    }
}
