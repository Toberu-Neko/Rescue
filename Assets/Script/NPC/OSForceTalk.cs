using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueTrigger))]
public class OSForceTalk : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;
    bool entered;
    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        entered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !entered)
        {
            entered = true;
            dialogueTrigger.TriggerDialouge();
        }
    }
}
