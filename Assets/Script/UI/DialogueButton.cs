using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueButton : MonoBehaviour
{
    private Button button;
    private DialogueManager dialogueManager;
    private void Start()
    {
        button = GetComponent<Button>();
        dialogueManager = UIManager.instance.dialogueManager;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        button.onClick.AddListener(SkipDialogue);
    }
    void SkipDialogue()
    {
        dialogueManager.EndDialogue();
    }
}
