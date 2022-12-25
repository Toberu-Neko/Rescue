using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuDialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject confirmSkipUI;
    [SerializeField] private PlayerData playerData;

    private Queue<string> sentences;
    public bool dialogueEnded;
    bool sentenceFinished;
    bool skipAnimation;
    float skipDialogueTimer;

    void Start()
    {
        skipDialogueTimer = 0;
        dialogueEnded = true;
        confirmSkipUI.SetActive(false);
        sentences = new Queue<string>();
    }
    private void Update()
    {
        if (!dialogueEnded && skipDialogueTimer <= 0.05f)
        { 
            skipDialogueTimer += Time.deltaTime;
        }
        if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Mouse0) && sentenceFinished && !dialogueEnded && !confirmSkipUI.activeInHierarchy)
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
                DisplayNextSentence();

            if(SceneManager.GetActiveScene().buildIndex == 2)
                DisplayNextSentence();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !sentenceFinished)
        {
            skipAnimation = true;
        }
        else if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Space) && !confirmSkipUI.activeInHierarchy && !dialogueEnded && SceneManager.GetActiveScene().buildIndex == 0)
        {
            skipDialogueTimer = 1f;
            confirmSkipUI.SetActive(true);
        }
    }
    public void StartDialogue(DialogueScriptableObj dialogue)
    {
        skipDialogueTimer = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sentenceFinished = true;
        skipAnimation = false;
        dialogueEnded = false;

        Time.timeScale = 0f;


        sentences.Clear();

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
                AudioManager.instance.Play("Typing");
                yield return new WaitForSecondsRealtime(0.005f);
            }
            else
            {
                dialogueText.text += letter;
                AudioManager.instance.Play("Typing");
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
        dialogueEnded = true;
        Time.timeScale = 1f;
    }
}
