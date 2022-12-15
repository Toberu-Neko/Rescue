using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    public bool died;
    public int maxHealth;
    private int currentHealth;
    public HealthBar enemyHealthBar;
    public GameObject enemyHealthBarObj;
    float combatTimer = 0;
    bool inCombat;
    void Start()
    {
        died = false;
        inCombat = false;
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
        enemyHealthBar.SetHealth(currentHealth);
        enemyHealthBarObj.SetActive(false);
    }

    void Update()
    {
        if (inCombat)
        {
            enemyHealthBarObj.SetActive(true);
            combatTimer += Time.deltaTime;
        }
        if (combatTimer > 3f)
        {
            inCombat = false;
            enemyHealthBarObj.SetActive(false);
            combatTimer = 0;
        }
    }
    public void EnemyTakeDamage(int damage)
    {
        inCombat = true;
        combatTimer = 0;
        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);
        //Play hurt animation

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Died();
        }
    }
    void Died()
    {
        //Die animation
        died = true;
        //Disable enemy
        GetComponent<Collider>().enabled = false;
        if (GetComponent<EnemyController>() != null)
            GetComponent<EnemyController>().enabled = false;
        else if (GetComponent<WaterEnemyController>() != null)
            GetComponent<WaterEnemyController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject, 5f);
        //StartCoroutine(DieDelay());
        
    }
    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        this.enabled = false;
    }
}
