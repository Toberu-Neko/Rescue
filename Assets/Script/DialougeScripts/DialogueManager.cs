using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    private GameObject confirmSkipUI;
    private GameObject pressSpaceNote;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;
    private GameObject dialougeUI;

    private GameObject goalParent;
    private TextMeshProUGUI goalText;
    //public DialogueScriptableObj dialogueKeeper; 
    private Animator animator;
    private PlayerStates playerStates;

    private Queue<string> sentences, npcNameArray;
    public bool dialoguePlaying;
    //public bool dialogueEnded;
    public bool ended;
    bool sentenceFinished;
    bool skipAnimation;
    bool skipAble;
    void Start()
    {
        confirmSkipUI = UIManager.instance.UI.transform.Find("Dialogue/ConfirmSkip").gameObject;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        nameText = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)/NPC Name").gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)/Dialouge").gameObject.GetComponent<TextMeshProUGUI>();
        pressSpaceNote = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)/「Space」來跳過劇情。").gameObject;
        dialougeUI = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)").gameObject;
        animator = UIManager.instance.UI.transform.Find("Dialogue/DialougeUI(BG)").gameObject.GetComponent<Animator>();
        goalText = UIManager.instance.UI.transform.Find("HUD/GoalHUD/現在目標內容Text").gameObject.GetComponent<TextMeshProUGUI>();
        goalParent = UIManager.instance.UI.transform.Find("HUD/GoalHUD/").gameObject;

        ended = true;
        dialoguePlaying = false;
        dialougeUI.SetActive(true);
        confirmSkipUI.SetActive(false);
        sentences = new Queue<string>();
        npcNameArray = new Queue<string>();

        if (goalText.text.Length != 0)
        {
            goalParent.SetActive(true);
        }
        else if(goalText.text.Length == 0)
            goalParent.SetActive(false);

    }
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Mouse0) && sentenceFinished && !ended && !confirmSkipUI.activeInHierarchy)
        {
            DisplayNextSentence();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && !sentenceFinished && !confirmSkipUI.activeInHierarchy)
        {
            skipAnimation = true;
        }
        else if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Space) && !ended && skipAble)
        {
            confirmSkipUI.SetActive(true);
        }
    }
    public void StartDialogue(DialogueScriptableObj dialogue)
    {
        if (dialogue.isTutorial)
        {
            skipAble = false;
            pressSpaceNote.SetActive(false);
        }
        else if (!dialogue.isTutorial)
        {
            skipAble = true;
            pressSpaceNote.SetActive(true);
        }
        Time.timeScale = 0f;
        goalParent.SetActive(false);
        //dialogueKeeper = dialogue;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ended = false;

        dialoguePlaying = true;
        sentenceFinished = true;
        skipAnimation = false;

        animator.SetBool("isOpen", true);
        //nameText.text = dialogue.npcName;
        
        sentences.Clear();
        npcNameArray.Clear();

        foreach(Line line in dialogue.lines)
        {
            npcNameArray.Enqueue(line.ncpName);
            sentences.Enqueue(line.sentences);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        string name = npcNameArray.Dequeue();
        StopAllCoroutines();
        nameText.text = name;
        StartCoroutine(TypeSentence(sentence));
        //dialogueText.text = sentence;
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceFinished = false;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            if (skipAnimation)
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(0.001f);
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
        goalParent.SetActive(true);
        playerStates.talkingTo = QuestGoal.TalkToTarget.None;
        dialoguePlaying = false;
        animator.SetBool("isOpen", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ended = true;
        Time.timeScale = 1f;
    }
    public void ChangeGoal(string goal)
    {
        goalText.text = goal;
        playerStates.nowGoal = goal;
    }
}
