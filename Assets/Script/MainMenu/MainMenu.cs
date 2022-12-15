using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private LevelLoader levelLoader;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private DialogueScriptableObj[] mainMenuDialogue;
    [SerializeField] private CanvasGroup[] diaCanvasGroup;
    [SerializeField] private CanvasGroup lastBlackImage;
    private GameObject dialogueTextGameobj;
    private MainMenuDialogueManager mainMenuDialogueManager;
    int dialogueCount;
    bool started;
    bool blackImagAlphaChanging;
    private void Start()
    {
        dialogueCount = 0;
        blackImagAlphaChanging = false;
        started = false;
        mainMenuDialogueManager = dialogue.GetComponent<MainMenuDialogueManager>();
        levelLoader = GetComponent<LevelLoader>();
        dialogueTextGameobj = dialogue.transform.Find("DialougeText").gameObject;
        //dialogueTextGameobj.SetActive(false);
    }
    private void Update()
    {
        if(mainMenuDialogueManager.dialogueEnded && dialogueCount < mainMenuDialogue.Length && !started)
        {

            dialogueTextGameobj.SetActive(false);
            mainMenuDialogueManager.dialogueEnded = false;
            Debug.Log(dialogueCount);
            StopAllCoroutines();
            StartCoroutine(CanvasGroupAlpha(dialogueCount));
            //Debug.Log(dialogueCount + "," + mainMenuDialogue.Length);

        }
        if(dialogueCount == mainMenuDialogue.Length  && !started && mainMenuDialogueManager.dialogueEnded)
        {
            if(lastBlackImage.alpha == 0)
            {
                dialogueTextGameobj.SetActive(false);
            }
            if (lastBlackImage.alpha < 1 && !blackImagAlphaChanging)
            {
                blackImagAlphaChanging = true;
                StopAllCoroutines();
                StartCoroutine(BlackImageAlpha());
            }
            if(lastBlackImage.alpha == 1)
            {
                StartGameLoadScene();
                started = true;
            }
        }
    }
    public void StratGameButton()
    {
        dialogue.SetActive(true);
        StartCoroutine(CanvasGroupAlpha(0));
    }
    IEnumerator BlackImageAlpha()
    {
        lastBlackImage.alpha += .1f;
        yield return new WaitForSeconds(0.05f);
        blackImagAlphaChanging = false;
    }
    IEnumerator CanvasGroupAlpha(int i)
    {
        while (diaCanvasGroup[i].alpha < 1)
        {
            diaCanvasGroup[i].alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        //Debug.Log(i);
        yield return null;
        dialogueTextGameobj.SetActive(true);
        dialogueCount++;
        mainMenuDialogueManager.StartDialogue(mainMenuDialogue[i]);
        yield return null;
    }
    public void TestGameLoadScene()
    {
        levelLoader.LoadLevel(3);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartGameLoadScene()
    {
        levelLoader.LoadLevel(1);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}