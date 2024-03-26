using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterDialogue : MonoBehaviour
{
    [SerializeField] DialogueScriptableObj dialogue;
    private DialogueManager dialogueManager;
    private bool playerInRange;
    private ForceMotionNew forceMotionNew;
    void Start()
    {
        forceMotionNew = PlayerManager.instance.player.GetComponent<ForceMotionNew>();
        dialogueManager = UIManager.instance.dialogueManager;
        playerInRange = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !playerInRange && forceMotionNew.isGrounded)
        {
            if (dialogue.haveGoal)
            {
                dialogueManager.ChangeGoal(dialogue.localizedNowGoal.GetLocalizedString());
            }
            dialogueManager.StartDialogue(dialogue);
            playerInRange = true;
        }
    }
}
