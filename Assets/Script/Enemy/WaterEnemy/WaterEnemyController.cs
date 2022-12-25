using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaterEnemyController : MonoBehaviour
{
    [Header("攻擊模式調整")]
    public int attackAmount;
    public float attackCooldown;
    public float attackGap;
    public float maxAttackAngle;
    public int attackDamage;
    public float throwForce;
    public float waterBallDieTime;

    [Header("遊走與偵測")]
    public float lookRadius;
    public float attackRadius;
    public float walkPointRange;
    public float maxPatrolDistance;

    [Header("附加物件")]
    public Transform attackDetector;
    public LayerMask playerLayer, ground;
    public GameObject waterBall;
    private WaterEnemyAttack waterEnemyAttack;
    bool readyToThrow;

    float searchWalkPointResetTimer;
    private Vector3 startPosition;
    private Vector3 walkPoint;
    bool walkPointSet;
    bool turnAble;
    PlayerStates playerStates;
    Transform target;
    NavMeshAgent agent;

    void Start()
    {
        searchWalkPointResetTimer = 0;
        readyToThrow = true;
        turnAble = true;
        startPosition = transform.position;
        waterEnemyAttack = waterBall.GetComponent<WaterEnemyAttack>();
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        waterEnemyAttack.damage = attackDamage;
        waterEnemyAttack.dieTime = waterBallDieTime;
    }


    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= attackRadius)
            {
                agent.SetDestination(transform.position);
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

            if (searchWalkPointResetTimer >= 10)
            {
                SearchWalkPoint();
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 4f)
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

            if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && Vector3.Distance(walkPoint, transform.position) >10f)
            {
                walkPointSet = true;
                searchWalkPointResetTimer = 0;
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
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 6f);


        Vector3 playerDirection = (playerStates.transform.position - attackDetector.position).normalized;

        if (readyToThrow && Physics.Raycast(attackDetector.position, playerDirection, out RaycastHit hit, attackRadius, playerLayer) && Vector3.Angle(playerDirection, transform.forward) < maxAttackAngle)
        {
            //Debug.Log("Facing player!");
            readyToThrow = false;
            turnAble = false;

            Vector3 throwDirection = (hit.point - attackDetector.position).normalized;
            //StopAllCoroutines();
            StartCoroutine(ThrowWaterBall(throwDirection));
            //hit.point
        }
    }
    IEnumerator ThrowWaterBall(Vector3 throwDirection)
    {
        //int objCount = 0;
        int attackCount = attackAmount;
        while(attackCount > 0)
        {
            SummonWaterBall(throwDirection);
            attackCount--;
            yield return new WaitForSeconds(attackGap);
        }

        yield return new WaitForSeconds(attackCooldown);
        readyToThrow = true;
        turnAble = true;
    }
    void SummonWaterBall(Vector3 throwDirection)
    {
        GameObject throwObj = Instantiate(waterBall, attackDetector.position, Quaternion.identity);
        Rigidbody throwObjRig = throwObj.GetComponent<Rigidbody>();
        //throwObj.transform.SetParent(transform);
        throwObjRig.velocity = (throwDirection * throwForce);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        if (attackDetector == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
