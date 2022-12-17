using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyInRange))]
public class DialogueTrigger : MonoBehaviour
{

    //public Dialogue dialogueOld;
    [Tooltip("放置只會顯示一次的對話，重開遊戲或ReloadScene會再次啟動，修正需要修改存檔系統，不推薦使用。")]
    public DialogueScriptableObj[] normalDialogue;

    [Tooltip("放置會不斷重複的對話")]
    public DialogueScriptableObj[] repeatDialogue;

    public DialogueScriptableObj giveQuestDia;
    public DialogueScriptableObj completeQuestDia;

    private EnemyInRange enemyInRange;
    private DialogueManager dialogueManager;
    private GameObject notePressE;
    private TextMeshProUGUI notePressEText;

    private PlayerStates playerStates;
    private PlayerData playerData;
    private QusetControler qusetControler;
    private PlayerSaveScript playerSaveScript;
    public KeyCode interactionKey = KeyCode.E;
    bool playerInRange;
    int dialogueCounter;
    int repeatDialogueCounter;
    
    private void Start()
    {
        dialogueCounter = 0;
        repeatDialogueCounter = 0;
        playerSaveScript = UIManager.instance.UI.GetComponent<PlayerSaveScript>();
        qusetControler = UIManager.instance.qusetControler;
        dialogueManager = UIManager.instance.dialogueManager;
        notePressE = PlayerManager.instance.player.transform.Find("WorldSpaceCanvas/PressEToTalkNote").gameObject;
        notePressEText = notePressE.transform.Find("PressEToTalk").gameObject.GetComponent<TextMeshProUGUI>();
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        playerData = PlayerManager.instance.playerData;
        enemyInRange = GetComponent<EnemyInRange>();
        notePressE.SetActive(false);
        if(repeatDialogue.Length == 0)
        {
            //Debug.LogError(name + " 的不斷重複對話未附加，會導致對話判定對象錯誤。");
        }


    }
    private void Update()
    {
        if(Input.GetKeyDown(interactionKey) && playerInRange && Cursor.lockState == CursorLockMode.Locked && !enemyInRange.enemyInRange)
        {
            TriggerDialouge();
        }
        if (enemyInRange.enemyInRange && playerInRange && notePressEText.text != "清除小怪")
        {
            notePressEText.text = "清除小怪";
        }
        if (!enemyInRange.enemyInRange && playerInRange && notePressEText.text != "「E」互動")
        {
            notePressEText.text = "「E」互動";
        }
    }
    #region 進入範圍E互動
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag ("Player"))
        {            
            playerInRange = true;
            notePressE.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            notePressE.SetActive(false);
        }
    }
    #endregion
    public void TriggerDialouge()
    {
        notePressE.SetActive(false);
        //playerStates.talkingTo = repeatDialogue[0].NpcName;

        /*if (playerStates.quest != null && playerStates.quest.goal.goalType == QuestGoal.GoalType.Talk && playerStates.talkingTo == playerStates.quest.goal.talkTo)
        {
            playerStates.quest.goal.Talked();
            if (playerStates.quest.goal.IsReached())
            {
                dialogueManager.StartDialogue(completeQuestDia);
                //quest completed and write save
                playerStates.quest.completed = true;
                //playerStates.questFinished[playerStates.quest.index] = true;
                playerStates.quest.isActive = false;

                //give reward
                GiveSkill();
                playerSaveScript.AutoSave();


                playerStates.quest = null;
            }
        }
        else if (giveQuestDia != null && giveQuestDia.quest.completed == false && (playerStates.quest.isActive == false || playerStates.quest == null))
        {
            //承接任務
            dialogueManager.StartDialogue(giveQuestDia);
            playerStates.quest = giveQuestDia.quest;
            qusetControler.QusetAccepted(giveQuestDia.quest);
        }*/
        if (dialogueCounter < normalDialogue.Length)
        {
            if (normalDialogue[dialogueCounter].completed)
            {
                dialogueManager.StartDialogue(repeatDialogue[repeatDialogueCounter]);
                repeatDialogueCounter++;
                if (repeatDialogueCounter == repeatDialogue.Length)
                {
                    repeatDialogueCounter = 0;
                }
            }
            if (!normalDialogue[dialogueCounter].completed)
            {
                dialogueManager.StartDialogue(normalDialogue[dialogueCounter]);
                normalDialogue[dialogueCounter].completed = true;
            }


            if (normalDialogue[dialogueCounter].haveGoal)
            {
                dialogueManager.ChangeGoal(normalDialogue[dialogueCounter].nowGoal);
            }
            if (normalDialogue[dialogueCounter].giveRegen)
            {
                playerStates.regenAble = true;
                playerData.regen = true;
            }

            if (normalDialogue[dialogueCounter].giveSwim)
            {
                playerStates.swimAble = true;
                playerData.swim = true;
            }

            if (normalDialogue[dialogueCounter].giveThrowStone)
            {
                playerStates.throwStoneAble = true;
                playerData.throwStone = true;
            }

            if (normalDialogue[dialogueCounter].giveFire)
            {
                playerStates.throwFireAble = true;
                playerData.throwFire = true;
            }

            if (normalDialogue[dialogueCounter].giveDebug)
            {
                playerStates.throwDebugAble = true;
                playerData.throwDebug = true;
            }


            dialogueCounter++;
        }
        else
        {
            dialogueManager.StartDialogue(repeatDialogue[repeatDialogueCounter]);
            repeatDialogueCounter++;
            if(repeatDialogueCounter == repeatDialogue.Length)
            {
                repeatDialogueCounter = 0;
            }
        }


        /*if (normalDialogue[dialogueCounter].dialogueName == "給予游泳能力" && !playerStates.swimAble)
        {
            playerStates.swimAble = true;
        }
        if (normalDialogue[dialogueCounter].dialogueName == "給予治癒能力" && !playerStates.regenAble)
        {
            playerStates.regenAble = true;
        }
        if (normalDialogue[dialogueCounter].dialogueName == "給予投石能力" && !playerStates.throwStoneAble)
        {
            playerStates.throwStoneAble = true;
            playerCombat.SelectSkill(1);
        }*/
    }


    void GiveSkill()
    {
        if (playerStates.quest.giveSwim)
            playerStates.swimAble = true;
        if (playerStates.quest.giveRegen)
            playerStates.regenAble = true;
        if(playerStates.quest.giveThrowStone)
            playerStates.throwStoneAble = true;
    }
}
