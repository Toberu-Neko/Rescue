using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour
{
    private Button button;
    private DialogueManager dialogueManager;
    private void Start()
    {
        button = GetComponent<Button>();
        dialogueManager = UIManager.instance.dialogueManager;
        button.onClick.AddListener(SkipDialogue);
    }
    void SkipDialogue()
    {
        dialogueManager.EndDialogue();
    }
}
