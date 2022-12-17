using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuDialogueManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject confirmSkipUI;

    private Queue<string> sentences;
    public bool dialogueEnded;
    bool sentenceFinished;
    bool skipAnimation;
    float skipDialogueTimer;

    void Start()
    {
        skipDialogueTimer = 0;
        //dialogueText = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)/Dialouge").gameObject.GetComponent<TextMeshProUGUI>();
        //dialougeUI = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)").gameObject;
        //animator = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)").gameObject.GetComponent<Animator>();
        //ended = false;
        //dialogueEnded = false;
        //dialougeUI.SetActive(true);
        dialogueEnded = true;
        confirmSkipUI.SetActive(false);
        sentences = new Queue<string>();
        //npcNameArray = new Queue<string>();

        //dialougeUI.SetActive(false);
    }
    private void Update()
    {
        if (!dialogueEnded && skipDialogueTimer <= 0.25f)
        { 
            skipDialogueTimer += Time.deltaTime;
        }
        if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Mouse0) && sentenceFinished && !dialogueEnded && !confirmSkipUI.activeInHierarchy)
        {
            DisplayNextSentence();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !sentenceFinished)
        {
            skipAnimation = true;
        }
        else if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Space) && !dialogueEnded && skipDialogueTimer >= 0.25f)
        {
            confirmSkipUI.SetActive(true);
        }
    }
    public void StartDialogue(DialogueScriptableObj dialogue)
    {
        skipDialogueTimer = 0;
        //dialogueKeeper = dialogue;
        //Debug.Log("InDialogue");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sentenceFinished = true;
        skipAnimation = false;
        dialogueEnded = false;

        //animator.SetBool("isOpen", true);
        //nameText.text = dialogue.npcName;

        sentences.Clear();
        //npcNameArray.Clear();

        foreach (Line line in dialogue.lines)
        {
            sentences.Enqueue(line.sentences);
        }


        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        
        StopAllCoroutines();
        
        StartCoroutine(TypeSentence(sentence));
        //dialogueText.text = sentence;
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceFinished = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (skipAnimation)
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(0.005f);
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        sentenceFinished = true;
        skipAnimation = false;
    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        sentences.Clear();
        //dialougeUI.SetActive(false);
        //animator.SetBool("isOpen", false);
        dialogueEnded = true;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //ended = true;
        Time.timeScale = 1f;
    }
}
