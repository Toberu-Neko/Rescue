using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossStates : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private HealthBar bossHealthBar;
    private GameObject bossHealthBarObj;
    private TestBossController bossController;
    [SerializeField] private GameObject diedParticle;
    [HideInInspector] public bool invincible;

    
    [Header("Change material")]
    private Renderer bossRenderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material attackedMaterial;

    //bool damaged;
    // Start is called before the first frame update
    void Start()
    {
        bossHealthBarObj = UIManager.instance.UI.transform.Find("HUD/BossHealthBar").gameObject;
        bossHealthBar = bossHealthBarObj.GetComponent<HealthBar>();
        bossController = GetComponent<TestBossController>();
        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);
        bossHealthBar.SetHealth(currentHealth);
        bossHealthBarObj.SetActive(false);

        bossRenderer = transform.Find("Design/BlueBoss").gameObject.GetComponent<Renderer>();
        //damaged = false;
        invincible = false;
    }
    public void BossTakeDamage(int damage)
    {
        if (invincible)
            return;

        ChangeAttackedMaterial();
        Invoke(nameof(ChangeNormalMaterial), 1f);
        currentHealth -= damage;
        bossHealthBar.SetHealth(currentHealth);
    

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Died();
            return;
        }
        bossController.damageCount += damage;
    }

    public void ChangeNormalMaterial()
    {
        bossRenderer.sharedMaterial = normalMaterial;
    }
    public void ChangeAttackedMaterial()
    {
        CancelInvoke(nameof(ChangeNormalMaterial));
        bossRenderer.sharedMaterial = attackedMaterial;
    }
    void Died()
    {
        //Die animation
        GameObject particle = Instantiate(diedParticle, transform.position, diedParticle.transform.rotation);
        Destroy(particle, .3f);
        transform.Find("Design").gameObject.SetActive(false);
        //Disable enemy
        GetComponent<Collider>().enabled = false;
        GetComponent<TestBossController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        HealthBarDeactive();
        //GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(DieDelay());

    }
    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        this.enabled = false;
    }


    public void HealthBarActive()
    {
        bossHealthBar = bossHealthBarObj.GetComponent<HealthBar>();
        bossHealthBarObj.SetActive(true);
        bossHealthBar.SetHealth(currentHealth);
    }
    public void HealthBarDeactive()
    {
        bossHealthBarObj.SetActive(false);
    }
}
