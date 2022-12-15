using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterEnemyAttack : MonoBehaviour
{
    public int damage;
    public float dieTime;
    PlayerStates playerStates;
    GameObject hitParticle;
    //bool damaged;
    void Start()
    {
        //playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        hitParticle = ParticleManager.instance.hitParticle;
        Destroy(gameObject, dieTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStates = other.transform.parent.parent.GetComponent<PlayerStates>();
            //playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
            GameObject particle = Instantiate(hitParticle, transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            playerStates.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
