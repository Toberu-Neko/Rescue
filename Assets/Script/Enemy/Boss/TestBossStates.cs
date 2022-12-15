using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBossStates : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private HealthBar bossHealthBar;
    private GameObject bossHealthBarObj;
    private TestBossController bossController;

    public bool invincible;

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

        invincible = false;
    }
    public void BossTakeDamage(int damage)
    {
        if (invincible)
            return;

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

    void Died()
    {
        //Die animation

        //Disable enemy
        GetComponent<Collider>().enabled = false;
        GetComponent<TestBossController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        healthBarDeactive();
        //GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(DieDelay());

    }
    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        this.enabled = false;
    }


    public void healthBarActive()
    {
        bossHealthBarObj.SetActive(true);
    }
    public void healthBarDeactive()
    {
        bossHealthBarObj.SetActive(false);
    }
}
