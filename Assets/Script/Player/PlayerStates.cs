using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private PlayerData playerData;
    public int maxHealth;
    public int currentHealth;

    //quest
    public Quest quest;
    public QuestGoal.TalkToTarget talkingTo;
    //public bool[] questFinished;

    public float playTime;

    //goal
    public string nowGoal;

    //skill
    public int skillSlot;
    public bool swimAble;
    public bool regenAble;
    public bool throwStoneAble;
    public bool throwFireAble;
    public bool throwDebugAble;

    //�{�ɪ��A
    public bool saveAble;
    public bool isAiming;
    public bool isCrouching;
    public bool died;
    public HealthBar healthBar;
    public bool regening;

    private float takeDamageTimer = 0;
    private bool regenComplete;

    [HideInInspector]
    public int currentScene;

    private TextMeshProUGUI goalText;

    void Start()
    {
        //questFinished = new bool[QuestManager.instance.dialogueScriptableObjs.Length];
        playerData = PlayerManager.instance.playerData;
        talkingTo = QuestGoal.TalkToTarget.None;
        goalText = UIManager.instance.UI.transform.Find("HUD/GoalHUD/�{�b�ؼФ��eText").gameObject.GetComponent<TextMeshProUGUI>();

        currentScene = playerData.currentScene;
        playTime = playerData.playTime;

        maxHealth = playerData.maxHealth;
        currentHealth = playerData.currentHealth;
        transform.position = playerData.position;

        nowGoal = playerData.nowGoal;
        goalText.text = playerData.nowGoal;

        //skill
        skillSlot = playerData.skillSlot;
        swimAble = playerData.swim;
        regenAble = playerData.regen;
        throwFireAble = playerData.throwFire;
        throwStoneAble = playerData.throwStone;
        throwDebugAble = playerData.throwDebug;

        regening = false;
        saveAble = false;
        isCrouching = false;
        isAiming = false;
        died = false;
        regenComplete = true;
        takeDamageTimer = 10;

        GetComponent<PlayerCombat>() .SelectSkill(skillSlot);
        healthBar = UIManager.instance.UI.transform.Find("HUD/HealthBar").gameObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    void Update()
    {
        playTime += Time.unscaledDeltaTime;
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(20);
        }*/
        if (currentHealth % 1 != 0)
        {
            currentHealth -= currentHealth % 1;
        }
        if (regenAble && takeDamageTimer < 11)
        {
            regening = false;
            takeDamageTimer += Time.deltaTime;
        }
        if(takeDamageTimer >= 10f)
        {
            regening = true;
        }

        if (regenAble && takeDamageTimer >= 10f && regenComplete && currentHealth < maxHealth) 
        {
            
            regenComplete = false;
            StartCoroutine(RegenDelay());
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        takeDamageTimer = 0;

        if(currentHealth <= 0)
        {
            Died();
        }
    }
    void Died()
    {
        died = true;
    }
    IEnumerator RegenDelay()
    {
        currentHealth += 1;
        healthBar.SetHealth(currentHealth);
        yield return new WaitForSeconds(.2f);
        regenComplete = true;
    }
}