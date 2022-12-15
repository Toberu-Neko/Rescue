using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossAttack : MonoBehaviour
{
    //public float attackPrepareTime;
    
    [SerializeField] LayerMask attackStop;


    private bool playerInRange;
    private bool dealDamage;
    //private Renderer thisRenderer;
    private Rigidbody rig;
    private PlayerStates playerStates;
    [HideInInspector] public float attackRadius;
    [HideInInspector] public int attackDamage;
    GameObject hitParticle;
    GameObject hitGroundParticle;

    void Start()
    {
        hitParticle = ParticleManager.instance.hitParticle;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        dealDamage = false;
        rig = GetComponent<Rigidbody>();
        playerInRange = false;
        Destroy(gameObject, 5f);
        //thisRenderer = GetComponent<Renderer>();
        //StartCoroutine(Attack());

    }
    private void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, attackRadius, attackStop))
        {
            rig.isKinematic = true;
            Destroy(gameObject);
        }
        if (playerInRange && !dealDamage)
        {
            GameObject particle = Instantiate(hitParticle, playerStates.transform.position, Quaternion.identity);
            Destroy(particle, 1f);
            playerStates.TakeDamage(attackDamage);
            dealDamage = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player") )
        {
            //Debug.Log("In!");
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    /*IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackPrepareTime - 0.2f);
        thisRenderer.material.color = Color.black;
        if (playerInRange)
        {
            PlayerStates player = PlayerManager.instance.player.GetComponent<PlayerStates>();

            player.TakeDamage(attackDamage);
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }*/
}
