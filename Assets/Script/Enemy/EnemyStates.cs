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
    private Renderer enemyRenderer;
    private GameObject enemyModel;

    [SerializeField] private GameObject grassDied;
    [SerializeField] private GameObject waterDied;

    [SerializeField] private Material waterNormal;
    [SerializeField] private Material waterAttacked;
    [SerializeField] private Material grassNormal;
    [SerializeField] private Material grassAttacked;

    float combatTimer;
    bool inCombat;
    [HideInInspector] public bool damaged;
    void Start()
    {
        combatTimer = 0;
        enemyModel = transform.Find("¼Ò«¬").gameObject;
        enemyRenderer = transform.Find("¼Ò«¬/EM").gameObject.GetComponent<Renderer>();
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
        if (inCombat && !died)
        {
            if(!enemyHealthBarObj.activeInHierarchy)
            {
                enemyHealthBarObj.SetActive(true);
            }

            combatTimer += Time.deltaTime;
            Debug.Log(combatTimer);
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
            enemyRenderer.sharedMaterial = grassAttacked;
        else if (GetComponent<WaterEnemyController>() != null)
            enemyRenderer.sharedMaterial = waterAttacked;
        CancelInvoke(nameof(DamagedFalse));
        Invoke(nameof(DamagedFalse), 1f);
        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);
        //Play hurt animation

        if (currentHealth <= 0)
        {
            enemyHealthBarObj.SetActive(false);
            enemyModel.SetActive(false);

            currentHealth = 0;
            died = true;
            Died();

        }
    }
    public void DamagedFalse()
    {
        if (GetComponent<EnemyController>() != null)
            enemyRenderer.sharedMaterial = grassNormal;
        else if (GetComponent<WaterEnemyController>() != null)
            enemyRenderer.sharedMaterial = waterNormal;
    }
    void Died()
    {
        GetComponent<Collider>().enabled = false;
        if (GetComponent<EnemyController>() != null)
        {
            GetComponent<EnemyController>().enabled = false;
            GameObject particle = Instantiate(grassDied, transform.position, grassDied.transform.rotation);
            Destroy(particle, .3f);
        }
        else if (GetComponent<WaterEnemyController>() != null)
        {
            GetComponent<WaterEnemyController>().enabled = false;
            GameObject particle = Instantiate(waterDied, transform.position, grassDied.transform.rotation);
            Destroy(particle, .3f);
        }
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
