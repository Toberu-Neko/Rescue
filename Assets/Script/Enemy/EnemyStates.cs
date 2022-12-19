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
    private Renderer renderer;

    [SerializeField] private Material waterNormal;
    [SerializeField] private Material waterAttacked;
    [SerializeField] private Material grassNormal;
    [SerializeField] private Material grassAttacked;

    float combatTimer = 0;
    bool inCombat;
    [HideInInspector] public bool damaged;
    void Start()
    {
        renderer = transform.Find("¼Ò«¬/EM").gameObject.GetComponent<Renderer>();
        died = false;
        damaged = false;
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
        if (GetComponent<EnemyController>() != null)
            renderer.sharedMaterial = grassAttacked;
        else if (GetComponent<WaterEnemyController>() != null)
            renderer.sharedMaterial = waterAttacked;
        CancelInvoke(nameof(DamagedFalse));
        Invoke(nameof(DamagedFalse), 1f);
        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);
        //Play hurt animation

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Died();
        }
    }
    public void DamagedFalse()
    {
        if (GetComponent<EnemyController>() != null)
            renderer.sharedMaterial = grassNormal;
        else if (GetComponent<WaterEnemyController>() != null)
            renderer.sharedMaterial = waterNormal;
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
