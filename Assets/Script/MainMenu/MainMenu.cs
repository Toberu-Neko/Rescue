using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
    bool blackImagAlphaChanging;

    private void Awake()
    {
        if(Application.systemLanguage == SystemLanguage.Chinese)
        {
            ChangeLanguage(0);
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            ChangeLanguage(2);
        }
        else
        {
            ChangeLanguage(1);
        }
    }

    private void Start()
    {
        dialogueCount = 0;
        blackImagAlphaChanging = false;
        mainMenuDialogueManager = dialogue.GetComponent<MainMenuDialogueManager>();
        levelLoader = GetComponent<LevelLoader>();
        dialogueTextGameobj = dialogue.transform.Find("DialougeText").gameObject;
        //dialogueTextGameobj.SetActive(false);
    }
    private void Update()
    {
        if(mainMenuDialogueManager.dialogueEnded && dialogueCount < mainMenuDialogue.Length)
        {
            dialogueTextGameobj.SetActive(false);
            mainMenuDialogueManager.dialogueEnded = false;
            //Debug.Log(dialogueCount);
            StopAllCoroutines();
            StartCoroutine(CanvasGroupAlpha(dialogueCount));
        }
        if(dialogueCount == mainMenuDialogue.Length  && mainMenuDialogueManager.dialogueEnded && SceneManager.GetActiveScene().buildIndex == 0)
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
            }
        }
    }
    public void StratGameButton()
    {
        dialogue.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(CanvasGroupAlpha(0));
    }
    IEnumerator BlackImageAlpha()
    {
        while(lastBlackImage.alpha < 1)
        {
            lastBlackImage.alpha += .1f;
            yield return new WaitForSeconds(0.05f);
        }
        blackImagAlphaChanging = false;
    }
    IEnumerator CanvasGroupAlpha(int i)
    {
        //Debug.Log("Dia start");
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
    }
    public void TestGameLoadScene()
    {
        levelLoader.LoadLevel(3);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartGameLoadScene()
    {
        Time.timeScale = 1f;
        levelLoader.LoadLevel(1);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private bool active = false;

    public void ChangeLanguageButton(int index)
    {
        if (active)
            return;

        StartCoroutine(ChangeLanguage(index));
    }

    private IEnumerator ChangeLanguage(int index)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        active = false;
    }
}
